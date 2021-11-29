using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MainServer.Bots.Commands;

namespace MainServer.Bots
{
    public class TBot : IBot
    {
        TelegramBotClient _instance;
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _scope;

        public TBot(ILogger<TBot> logger, IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            _instance = new TelegramBotClient(configuration["BotToken"]);
            _logger = logger;
            _scope = scopeFactory;
        }


        public async Task Work()
        {

            var me = await _instance.GetMeAsync();
            _logger.LogInformation(
              $"Hello, World! I am bot {me.Id} and my name is {me.FirstName}."
            );

            using var cts = new CancellationTokenSource();

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            _instance.StartReceiving(
                new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync),
                cts.Token);

            _logger.LogInformation($"Start listening for @{me.Username}");

            // Send cancellation request to stop bot
            //cts.Cancel();
        }
        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogInformation(ErrorMessage);
            return Task.CompletedTask;
        }

        private async Task ProccessMessage(Message msg, ITelegramBotClient botClient, long chatId)
        {
            BotMessageValidator validator;
            using (var scope = _scope.CreateScope())
            {
                validator=ActivatorUtilities.CreateInstance<BotMessageValidator>(scope.ServiceProvider, msg);
                //validator = scope.ServiceProvider.GetRequiredService();

            }
            if (validator.IsValid)//если распознана комманда
            {
                try
                {
                    ICommand command= validator.GetCommand();
                    string text=command.Run(_scope);//выполнить команду
                    await botClient.SendTextMessageAsync(chatId: chatId, text: text);
                    //await Command(botClient, chatId, msg.From);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                }
            }
            else if(validator.ValidErrorMessage!="")
            {
                await ErrorMessage(botClient, chatId, validator.ValidErrorMessage);
            }
           
        }

        private async Task ErrorMessage(ITelegramBotClient botClient, long chatId, string msg)
        {
            await botClient.SendTextMessageAsync(
               chatId: chatId,
               text: msg
           );
        }

        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type != UpdateType.Message)
                return;
            if (update.Message.Type != MessageType.Text)
                return;

            long chatId = update.Message.Chat.Id;

            _logger.LogInformation($"Received a '{update.Message.Text}' message in chat {chatId}.");//logging

            await ProccessMessage(update.Message, botClient, chatId);
        }
    }
}
