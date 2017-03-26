using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Genoom.Simpsons.Model;
using Genoom.Simpsons.Repository;
using Genoom.Simpsons.Web.Support;

namespace Genoom.Simpsons.Web.Controllers
{
    [Route("[controller]")]
    public class PeopleController : Controller
    {
        // Properties
        protected IPeopleRepository RepositoryService { get; }

        // Ctor
        public PeopleController(IPeopleRepository repositoryService)
        {
            RepositoryService = repositoryService;
        }

        //Public Methods
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var result = await RepositoryService.GetPersonAsync(id);
                return result != null
                    ? (IActionResult) Ok(result)
                    : NoContent();
            }
            catch (Exception exception)
            {
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError,
                    ErrorResultHelper.Create(exception));
            }
        }

        [HttpGet("{id}/family")]
        public async Task<IActionResult> GetFamily(string id)
        {
            try
            {
                var result = await RepositoryService.GetFamilyAsync(id);
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

        [HttpPost("{id}/children")]
        public async Task<IActionResult> Post(string id, [FromBody]Person body)
        {
            try
            {
                if (!await RepositoryService.HasPartnerAsync(id)) {
                    return StatusCode(
                        (int)HttpStatusCode.PreconditionFailed,
                        ErrorResultHelper.Create(new Exception($"The person <{id}> has no partner. Associate one to it and try to add the child again."), HttpStatusCode.PreconditionFailed)
                    );
                }

                var result = await RepositoryService.AddChildAsync(id, body);
                return !string.IsNullOrEmpty(result)
                    ? (IActionResult)Created(GetNewChildPath(result), result)
                    : NoContent();
            }
            catch (Exception exception)
            {
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError,
                    ErrorResultHelper.Create(exception));
            }
        }

        // Private Methods
        private string GetNewChildPath(string newChildId)
        {
            return Request.PathBase.Add(newChildId);
        }
    }
}
