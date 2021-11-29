using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainServer.Models;

namespace MainServer.Repository
{
    public interface IPersonRepository<T>
    {
        IEnumerable<T> GetPersons();
        T GetPersonByUsername(string username);
        void AddPerson(T Person);
        void DeletePersonByUsername(string username);
        void DeletePerson(T Person);
        void UpdatePerson(T Person);
        void Save();

    }
}
