using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using MainServer.Repository;
using MainServer.Models;
using Microsoft.Extensions.Logging;
using MainServer.Bots.Commands;

namespace MainServer.Bots
{
    public enum Permission
    {
        GUEST,
        STUDENT,
        LECTURER
    } 

    public class BotMessageValidator
    {

        private Permission _permission = Permission.GUEST;
        private readonly IPersonRepository<Student> _sRepository;
        private readonly IPersonRepository<Lecturer> _lRepository;
        private CommandFactory _commandFactory =null;
        private ICommand _command = null;
        private readonly ILogger _logger;
        public string ValidErrorMessage = "";
        public Permission Permission { get => _permission; }
        public BotMessageValidator(Message msg, IPersonRepository<Student> SRepository,
    IPersonRepository<Lecturer> LRepository, ILogger<BotMessageValidator> logger)
        {
            _sRepository = SRepository;
            _lRepository = LRepository;
            _logger = logger;
            _permission = checkUserPermission(msg.From.Username);
            //if(_permission == Permission.Guest)
            //{
            //    ValidErrorMessage = $"User {msg.From.Username}, not registred";
            //    _logger.LogInformation(ValidErrorMessage);
            //}

            _commandFactory = new CommandFactory();
            _commandFactory.From = msg.From.Username;
            try
            {
                _command=_commandFactory.Parse(msg.Text);
            }
            catch (Exception e)
            {
                ValidErrorMessage = $"cant parse command {msg.Text}";
                _logger.LogInformation(ValidErrorMessage);
                return;
            }

            var perm = _command.access.FirstOrDefault(x => x.ToUpper().Equals(_permission.ToString()));
            if(perm==null)
            {
                ValidErrorMessage = $"permission of {msg.From.Username} not enough for /{_command.name}";
                _logger.LogInformation(ValidErrorMessage);
                return;
            }

            IsValid = true;
        }

        public ICommand GetCommand()
        {
            if (IsValid)
                return _command;
            else
                throw new Exception("NotValid");
        }

        public bool IsValid { get; } = false;
        private Permission checkUserPermission(string UserName)
        {
            IPerson user = _lRepository.GetPersonByUsername(UserName);
            if (user != null)
            {
               return Permission.LECTURER;
            }
            else
            {
                user = _sRepository.GetPersonByUsername(UserName);
                if (user != null)
                {
                    return Permission.STUDENT;
                }
                else
                {
                    return Permission.GUEST;
                }
            }
        }




    }
}
