using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainServer.Models
{
    public interface IPerson
    {
        //public Guid Id { get; set; }
        public string Username { get; set; }
        public string FIO { get; set; }
    }
}
