using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MainServer.Repository;
using MainServer.Models;

namespace MainServer.Bots.Commands
{
    public class RegistredListCommand : BaseCommand, ICommand
    {
        public List<string> required { get; set; }
        public List<string> getedArgs { get; set; }
        public string From { get; set; }
        public RegistredListCommand(BaseCommand b, string From)
        {
            this.From = From;
            foreach (var field in typeof(BaseCommand).GetProperties())
            {
                field.SetValue(this, field.GetValue(b));
            }
        }

        public void setup(List<string> getArgs)
        {
            getedArgs = getArgs;
            required = args.Except(optional).ToList();
        }

        bool checkArgs()
        {
            if (getedArgs.Count < required.Count)
                return false;

            return true;
        }


        string getAllLectors(IServiceScopeFactory _scope)
        {
            string res = "";
            using (var scope = _scope.CreateScope())
            {
                var lecturerRep = scope.ServiceProvider.GetRequiredService<IPersonRepository<Lecturer>>();
                var lecturers = lecturerRep.GetPersons();
                foreach (var l in lecturers)
                    res += $"{l.Username} {l.FIO} Lecturer\n";
            }
            return res;
        }
        string getAllStudents(IServiceScopeFactory _scope)
        {
            string res = "";
            using (var scope = _scope.CreateScope())
            {
                var studentRep = scope.ServiceProvider.GetRequiredService<IPersonRepository<Student>>();
                var students = studentRep.GetPersons();
                foreach (var l in students)
                    res += $"{l.Username} {l.FIO} Student\n";
            }
            return res;
        }
        public String Run(IServiceScopeFactory _scope)
        {
            String res = "";
            if (!checkArgs())
                return "cant run command with this args, try /help";

            try
            {   
                Permission who;
                if (getedArgs.Count == 1)
                    who = (Permission)Enum.Parse(typeof(Permission), getedArgs[0].ToUpper());
                else
                    who = Permission.GUEST;
                switch (who) { 
                    case Permission.LECTURER:
                        res=getAllLectors(_scope);
                        break;
                    case Permission.STUDENT:
                        res = getAllStudents(_scope);
                        break;
                    default:
                        res = getAllLectors(_scope) + getAllStudents(_scope);
                        break;
                }
                    
            }
            catch (ArgumentException e)
            {
                return $"Argument {getedArgs[0]} is unexpected to this command. Try /help";
            }
            catch (Exception e)
            {
                return "error";
            }
            
            
            return res;
        }
    }
}
