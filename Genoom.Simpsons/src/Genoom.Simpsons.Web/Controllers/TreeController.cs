﻿using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Genoom.Simpsons.Repository;
using Genoom.Simpsons.Web.Support;

namespace Genoom.Simpsons.Web.Controllers
{
    [Route("[controller]")]
    public class TreeController : Controller
    {
        // Properties
        protected IPeopleRepository RepositoryService { get; }

        // Ctor
        public TreeController(IPeopleRepository repositoryService)
        {
            RepositoryService = repositoryService;
        }

        //Public Methods
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var result = await RepositoryService.GetTreeAsync(id);
                return result != null
                    ? (IActionResult)Ok(result)
                    : NoContent();
            }
            catch (Exception exception)
            {
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError,
                    ErrorResultHelper.Create(exception));
            }
        }
    }
}
