using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainServer.Bots;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MainServer.Repository;
using MainServer.Models;

namespace MainServer.Bots.Commands
{
    public class RegisterMeCommand : BaseCommand, ICommand
    {
        public List<string> required { get; set; }
        public List<string> getedArgs { get; set; }
        public string From { get; set; }
        public RegisterMeCommand(BaseCommand b, string From)
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
            if (required.Count == 0)
                return true;

            if (getedArgs.Count == 0)
                return true;

            if (getedArgs.Count < required.Count)
                return false;

           //for (int i = 0; i < getedArgs.Count; i++)
           // {
           //     Type type = Assembly.GetExecutingAssembly().GetTypes().First(t => t.Name == typeArgs[i]); 
           //     try
           //     {
           //         Enum.Parse(type, getedArgs[i].ToUpper());
           //     }
           //     catch(Exception e)
           //     {
           //         return false;
           //     }
           // }

            return true;
        }

        IPersonRepository<Lecturer> lecturerRep;
        IPersonRepository<Student> studentRep;
        public String Run(IServiceScopeFactory _scope)
        {
            String res = " ";
            if (!checkArgs())
                return "cant run command with this args, try /help";
            var role = Enum.Parse(typeof(Permission), getedArgs[0].ToUpper());
            using (var scope = _scope.CreateScope())
            {
                lecturerRep= scope.ServiceProvider.GetRequiredService<IPersonRepository<Lecturer>>();
                studentRep = scope.ServiceProvider.GetRequiredService<IPersonRepository<Student>>();
            
            IPerson person;
            switch(role)
            {
                case (Permission.LECTURER):
                    person = new Lecturer();
                    person.FIO = getedArgs[1];
                    person.Username = From;
                    try
                    {
                        lecturerRep.AddPerson((Lecturer)person);
                    }
                    catch(Exception e)
                    {
                        return "registration error";
                    }
                    break;
                case (Permission.STUDENT):
                    person = new Student();
                    person.FIO = getedArgs[1];
                    person.Username = From;
                    try
                    {
                        studentRep.AddPerson((Student)person);
                    }
                    catch (Exception e)
                    {
                        return "registration error";
                    }
                    break;
            }
            }
            res = $"Succesfull refistred {From} as {role.ToString()}";
            return res;
        }
    }
}
