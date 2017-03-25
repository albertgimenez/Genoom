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
        public async Task<IActionResult> Get(Guid id)
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
        public async Task<IActionResult> GetFamily(Guid id)
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

        // POST api/values
        [HttpPost("{id}/children")]
        public async Task<IActionResult> Post(Guid id, [FromBody]Person body)
        {
            try
            {
                if (!await RepositoryService.HasPartnerAsync(id)) {
                    //Logger.LogError($"The person with <{id}> has no partner. Associate one to it and try to add the child again.");
                    return new StatusCodeResult((int)HttpStatusCode.PreconditionFailed);
                }

                var result = await RepositoryService.AddChildAsync(id, body);
                return result != Guid.Empty
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

        private string GetNewChildPath(Guid newChildId)
        {
            return Request.PathBase.Add(newChildId.ToString());
        }
    }
}
