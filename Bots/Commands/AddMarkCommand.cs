using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MainServer.Repository;
using MainServer.Models;
using System.Globalization;

namespace MainServer.Bots.Commands
{
    public class AddMarkCommand : BaseCommand, ICommand
    {
        public AddMarkCommand(BaseCommand b, string From)
        {
            this.From = From;
            foreach (var field in typeof(BaseCommand).GetProperties())
            {
                field.SetValue(this, field.GetValue(b));
            }
        }
        public List<string> required { get; set; }
        public List<string> getedArgs { get; set; }
        public string From { get; set; }
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
        IPersonRepository<Lecturer> lecturerRep;
        IPersonRepository<Student> studentRep;
        public String Run(IServiceScopeFactory _scope)
        {
            String res = " ";
            if (!checkArgs())
                return "cant run command with this args, try /help";

            string user = getedArgs[0];
            string discipline = getedArgs[1];
            Mark mark = new Mark();
           
            try
            {
                double value = double.Parse(getedArgs[2], System.Globalization.NumberStyles.Float, CultureInfo.InvariantCulture);
                mark.Value = value;
            }
            catch (Exception e)
            {
                return $"cant parse mark {getedArgs[2]} to double";
            }

            mark.Label = getedArgs.ElementAtOrDefault(3);
            

            using (var scope = _scope.CreateScope())
            {
                try
                {
                    lecturerRep = scope.ServiceProvider.GetRequiredService<IPersonRepository<Lecturer>>();
                    studentRep = scope.ServiceProvider.GetRequiredService<IPersonRepository<Student>>();
                    Lecturer lec = lecturerRep.GetPersonByUsername(From);

                    Student student = studentRep.GetPersonByUsername(user);
                    if (student == null)
                    {
                        return $"student {user} is not found. maybe use /help";
                    }
                    if(!lec.Students.Contains(student))
                    {
                        return $"student {user} is not in yours studentlist. \n " +
                            $"try /myStudents to see yours or add with /addStudent";
                    }

                    if (student.Marks.ContainsKey(discipline))
                    {
                        student.Marks[discipline].Add(mark);
                    }
                    else
                        student.Marks[discipline] = new List<Mark> { mark };

                    studentRep.UpdatePerson(student);
                }
                catch (Exception e)
                {
                    return "adding error";
                }
            }
            res = "Mark was added";
            return res;
        }
    }
}
