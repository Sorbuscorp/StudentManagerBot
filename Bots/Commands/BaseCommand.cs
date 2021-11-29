using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainServer.Bots.Commands
{
    public class BaseCommand
    {
        public string name { get  ; set  ; }
        public string code { get  ; set  ; }
        public string description { get  ; set  ; }
        public List<string> access { get  ; set  ; }
        public List<string> args { get  ; set  ; }
        public List<string> optional { get  ; set  ; }
        public List<string> typeArgs { get; set; }
    }
}
