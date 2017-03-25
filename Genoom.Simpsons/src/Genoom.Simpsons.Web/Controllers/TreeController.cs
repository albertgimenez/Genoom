using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Genoom.Simpsons.Repository;

namespace Genoom.Simpsons.Web.Controllers
{
    [Route("[controller]")]
    public class TreeController : Controller
    {
        // Properties
        protected ILogger Logger { get; }
        protected IPeopleRepository RepositoryService { get; }

        // Ctor
        public TreeController(ILogger logger, IPeopleRepository repositoryService)
        {
            Logger = logger;
            RepositoryService = repositoryService;
        }

        //Public Methods
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var result = await RepositoryService.GetTreeAsync(id);
                return result != null
                    ? (IActionResult)Ok(result)
                    : NotFound(id);
            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message);
                throw;
            }
        }
    }
}
