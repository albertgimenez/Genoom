using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Genoom.Simpsons.Model;

namespace Genoom.Simpsons.Web.Controllers
{
    [Route("[controller]")]
    public class PeopleController : Controller
    {
        // GET people/5
        [HttpGet("{id}")]
        public Person Get(Guid id)
        {
            return new Person { Id = id };
        }

        // GET api/5/family
        [HttpGet("{id}/family")]
        public IEnumerable<Person> GetFamily(Guid id)
        {
            return new Person[10];
        }

        // POST api/values
        [HttpPost("{id}/children")]
        public Guid Post(Guid id, [FromBody]Person body)
        {
            return Guid.NewGuid();
        }
    }
}
