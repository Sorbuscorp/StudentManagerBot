using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainServer.Models;
using MainServer.DBContexts;
using Microsoft.EntityFrameworkCore;

namespace MainServer.Repository
{
    public class LecturerRepository : IPersonRepository<Lecturer>
    {
        private readonly MainContext _dbContext;

        public LecturerRepository(MainContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddPerson(Lecturer Person)
        {
            _dbContext.Add(Person);
            Save();
        }

        public void DeletePersonByUsername(string username)
        {
            var person = _dbContext.Lecturers.Find(username);
            _dbContext.Lecturers.Remove(person);
            Save();
        }

        public void DeletePerson(Lecturer Person)
        {
            _dbContext.Lecturers.Remove(Person);
            Save();
        }

        public Lecturer GetPersonByUsername(string username)
        {
            return _dbContext.Lecturers.Find(username);
            
        }

        public IEnumerable<Lecturer> GetPersons()
        {
            return _dbContext.Lecturers.ToList();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void UpdatePerson(Lecturer Person)
        {
            _dbContext.Entry(Person).State = EntityState.Modified;
            Save();
        }
    }
}
