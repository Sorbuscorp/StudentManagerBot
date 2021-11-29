using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MainServer.DBContexts;
using MainServer.Repository;
using MainServer.Models;
using MainServer.Bots;
using System;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace MainServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddDbContext<MainContext>();
            services.AddTransient<IPersonRepository<Lecturer>, LecturerRepository>();
            services.AddTransient<IPersonRepository<Student>, StudentRepository>();
            services.AddSingleton<IBot, TBot>();
            services.AddTransient<Func<Message,BotMessageValidator>>((p)=> {
                return new Func<Message, BotMessageValidator>(
(       msg) => new BotMessageValidator(msg, p.GetRequiredService< IPersonRepository<Student>>(), 
p.GetRequiredService<IPersonRepository<Lecturer>>(), p.GetRequiredService<ILogger<BotMessageValidator>>())
);
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            var bot = app
                      .ApplicationServices
                      .GetRequiredService<IBot>();
            bot.Work();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
