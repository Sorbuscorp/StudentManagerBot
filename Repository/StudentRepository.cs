using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainServer.Models;
using MainServer.DBContexts;
using Microsoft.EntityFrameworkCore;

namespace MainServer.Repository
{
    public class StudentRepository : IPersonRepository<Student>
    {
        private readonly MainContext _dbContext;

        public StudentRepository(MainContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddPerson(Student Person)
        {
            _dbContext.Add(Person);
            Save();
        }

        public void DeletePersonByUsername(string guid)
        {
            var person = _dbContext.Students.Find(guid);
            _dbContext.Students.Remove(person);
            Save();
        }

        public void DeletePerson(Student Person)
        {
            _dbContext.Students.Remove(Person);
            Save();
        }

        public Student GetPersonByUsername(string guid)
        {
            return _dbContext.Students.Find(guid);

        }

        public IEnumerable<Student> GetPersons()
        {
            return _dbContext.Students.ToList();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void UpdatePerson(Student Person)
        {
            _dbContext.Entry(Person).State = EntityState.Modified;
            Save();
        }
    }
}
