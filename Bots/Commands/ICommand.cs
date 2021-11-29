using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace MainServer.Bots.Commands
{
    public interface ICommand
    {

        public List<string> required { get; set; }
        public List<string> getedArgs { get; set; }
        public List<string> access { get; set; }
        public string name { get  ; set  ; }
        public string From { get; set; }

        public void setup(List<string> getArgs);
        public String Run(IServiceScopeFactory _scope);

    }
}
