using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MainServer.Repository;
using MainServer.Models;

namespace MainServer.Bots.Commands
{
    public class RemoveStudentCommand : BaseCommand, ICommand
    {
        public List<string> required { get; set; }
        public List<string> getedArgs { get; set; }
        public string From { get; set; }
        public RemoveStudentCommand(BaseCommand b, string From)
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
            if (optional == null)
                required = args;
            else
                required = args.Except(optional).ToList();
        }

        bool checkArgs()
        {
            if (getedArgs.Count < required.Count)
                return false;
            return true;
        }


        public String Run(IServiceScopeFactory _scope)
        {
            String res = "";
            if (!checkArgs())
                return "cant run command with this args, try /help";

            try
            {
                using (var scope = _scope.CreateScope())
                {
                    var lecturerRep = scope.ServiceProvider.GetRequiredService<IPersonRepository<Lecturer>>();
                    var lector = lecturerRep.GetPersonByUsername(From);
                    List<Student> list = lector.Students.ToList();
                    if (list.RemoveAll(x => x.Username == getedArgs[0]) > 0)
                    {
                        lector.Students = list;
                        lecturerRep.UpdatePerson(lector);
                        res = $"student {getedArgs[0]} was deleted from you";
                    }
                    else
                    {
                        res = $"student {getedArgs[0]} not in your students";
                    }
                }
            }
            catch (Exception e)
            {
                return "error";
            }


            return res;
        }
    }
}
