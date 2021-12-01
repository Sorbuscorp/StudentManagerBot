using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainServer.Models
{
    public class Student : IPerson
    {
        public Student()
        {
            Marks = new Dictionary<string, List<Mark>>();
            Lecturers = new List<Lecturer>();
        }
        public string Group { get; set; }

        public Dictionary<string, List<Mark>> Marks { get; set; }
        public string Username { get; set; }
        public string FIO { get; set; }

        public string[] GetDisciplines()
        {
            return Marks.Select(rating => rating.Key).ToArray();

        }

        public ICollection<Lecturer> Lecturers { get; set; }
    }
}
