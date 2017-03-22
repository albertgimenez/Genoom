using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Genoom.Simpsons.Model;

namespace Genoom.Simpsons.Web.Controllers
{
    [Route("[controller]")]
    public class TreeController : Controller
    {
        // GET people/5
        [HttpGet("{id}")]
        public IEnumerable<Person> Get(Guid id)
        {
            return new Person[10];
        }
    }
}
