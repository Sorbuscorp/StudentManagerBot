using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MainServer.Repository;
using MainServer.Models;

namespace MainServer.Bots.Commands
{
    public class MyStudentsCommand : BaseCommand, ICommand
    {
        public List<string> required { get; set; }
        public List<string> getedArgs { get; set; }
        public string From { get; set; }

        public MyStudentsCommand(BaseCommand b, string From)
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
        List<Student> getAllStudents(IServiceScopeFactory _scope)
        {
            List<Student> res;
            using (var scope = _scope.CreateScope())
            {
                var lecturerRep = scope.ServiceProvider.GetRequiredService<IPersonRepository<Lecturer>>();
                var lector=lecturerRep.GetPersonByUsername(From);
                res=lector.Students.ToList();               
            }
            return res;
        }

        string ListToString(List<Student> students)
        {
            string res = "";
            foreach (var l in students)
                res += $"{l.Username}\t{l.FIO}\t{l.Group}\n";
            return res;
        }

        public String Run(IServiceScopeFactory _scope)
        {
            String res = "";
            if (!checkArgs())
                return "cant run command with this args, try /help";

            string filter="";
            string value="";
            if (getedArgs.Count > 1)
            {
                filter = getedArgs[0];
                value = getedArgs[1];
            }
            var AllStudent=getAllStudents(_scope);
            try
            {
                switch (filter)
                {
                    case "discipline":
                        AllStudent = AllStudent.Where(x => x.Marks.Keys.Contains(value)).ToList();
                        break;
                    case "group":
                        AllStudent = AllStudent.Where(x => x.Group == value).ToList();
                        break;
                }
                if (AllStudent.Count == 0)
                    return "Students Not found";

                res = ListToString(AllStudent);
            }
            catch (Exception e)
            {
                return "error";
            }
            
            
            return res;
        }
    }
}
