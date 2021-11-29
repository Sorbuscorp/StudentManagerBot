using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainServer.Models;
using MainServer.Repository;
using System.Transactions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MainServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LecturerController : ControllerBase
    {
        private readonly IPersonRepository<Lecturer> _repository;

        public LecturerController(IPersonRepository<Lecturer> repository)
        {
            this._repository = repository;
        }

        // GET: api/<LecturerController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<LecturerController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<LecturerController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<LecturerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<LecturerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
