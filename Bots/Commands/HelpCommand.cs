using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;

namespace MainServer.Bots.Commands
{
    public class HelpCommand : BaseCommand, ICommand
    {
        public List<BaseCommand> _allCommands { get; set; }
        public List<string> required { get; set; }
        public List<string> getedArgs { get ; set; }
        public string From { get; set ; }

        private string _fullText;

        public HelpCommand(BaseCommand b, string From)
        {
            this.From = From;
            _fullText = File.ReadAllText(@".\Bots\Commands.json");
            _allCommands = JsonConvert.DeserializeObject<List<BaseCommand>>(_fullText);
            foreach (var field in typeof(BaseCommand).GetProperties())
            {
                field.SetValue(this, field.GetValue(b));
            }
        }

        public void setup( List<string> getArgs)
        {
            
            getedArgs = getArgs;
            required = args.Except(optional).ToList();
        }

        public String Run(IServiceScopeFactory _scope)
        {
            String res;
            if (getedArgs.Count == 0)
                res = _fullText;
            else
            {
                var helped = _allCommands.Where(x => x.name == getedArgs.First()).ToList();
                if (helped.Count == 0)
                    return $"Cant find help for \"{getedArgs.First()}\"";

                if (helped.Count > 1)
                    throw new Exception("Shiiiit");

                res = JsonConvert.SerializeObject(helped.First()); ;
            }

            return res;
        }
    }
}
