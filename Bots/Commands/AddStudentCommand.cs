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
    public class AddStudentCommand : BaseCommand, ICommand
    {
        public List<string> required { get; set; }
        public List<string> getedArgs { get; set; }
        public string From { get; set; }
        public AddStudentCommand(BaseCommand b, string From)
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

            string user = getedArgs[0];
            string family = getedArgs[1];

            using (var scope = _scope.CreateScope())
            {
                try
                {
                    lecturerRep = scope.ServiceProvider.GetRequiredService<IPersonRepository<Lecturer>>();
                    studentRep = scope.ServiceProvider.GetRequiredService<IPersonRepository<Student>>();

                    Student student = studentRep.GetPersonByUsername(user);
                    if (student == null)
                    {
                        student = new Student();
                        student.FIO = family;
                        student.Username = user;
                        studentRep.AddPerson(student);
                    }
                    Lecturer lec = lecturerRep.GetPersonByUsername(From);
                    lec.Students.Add(student);
                    lecturerRep.UpdatePerson(lec);
                }
                catch (Exception e)
                {
                    return "adding error";
                }
            }
            res = $"Added {user} as student for you";
            return res;
        }
    }
}
