using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainServer.Models
{
    public class Lecturer : IPerson
    {
        public Lecturer()
        {
            Students = new List<Student>();
        }
        public ICollection<Student> Students { get; set; }
        public string Username { get; set ; }
        public string FIO { get; set; }
    }
}
