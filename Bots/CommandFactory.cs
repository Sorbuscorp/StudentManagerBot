using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using MainServer.Bots.Commands;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;

namespace MainServer.Bots
{
    
    public class CommandFactory
    {
        public List<BaseCommand> _availableCommands { get; private set; }
        public string getName { get; private set; }
        public List<string> getArgs { get; private set; }
        public string From { get; set; }

        public CommandFactory()
        {
            //Get args and optionals from Commands.json by CommandName, and check current args
            String json = File.ReadAllText(@".\Bots\Commands.json");
            _availableCommands = JsonConvert.DeserializeObject<List<BaseCommand>>(json);
        }

        public ICommand Parse(string Message)
        {
            getArgs=Regex.Split(Message, @"\s+").ToList();
            var match=Regex.Match(getArgs[0], @"/\w+") ;
            getName = match.Length == getArgs[0].Length ? match.Value : null; //если будет несколько ботов то стоит переписать условие
            if(getName==null)
            {
                throw new Exception("Can't parse");
            }

            getArgs.RemoveAt(0);

            var command=GenerateCommand();
            command.setup(getArgs);

            return command;
        }

        public ICommand GenerateCommand()
        {
            var commands = _availableCommands.Where(x => x.code == this.getName).ToList();

            if(commands.Count==0)
                throw new Exception("Can't parse");

            if(commands.Count >1)
                throw new Exception("Can't parse"); //this need change if add multiple command with same name proccessing

            var baseComm=commands.First();
            
            var commObjName = char.ToUpper(baseComm.name[0]) + baseComm.name.Substring(1) + "Command";

            
            Type type = Assembly.GetExecutingAssembly().GetTypes().First(t => t.Name == commObjName);

            return (ICommand)Activator.CreateInstance(type, baseComm, From);
        }



    }
 
}
