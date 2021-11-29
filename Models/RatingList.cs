using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainServer.Models
{
    public class RatingList
    {
        public string SubjectName { get; set; }

        public Mark[] Marks { get; set; }
    }
}
