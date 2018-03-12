using System.Collections.Generic;
using System.Web.Http;
using Kawaii.NetworkDocumentation.AppDataService.Managers;
using Kawaii.NetworkDocumentation.AppDataService.ServiceModel;

namespace Kawaii.NetworkDocumentation.AppDataService.Controllers
{
    [RoutePrefix("api/computers")]
    public class ComputersController : ApiController
    {
        private readonly IComputerManager computerManager;
                
        public ComputersController(IComputerManager computerManager)
        {
            this.computerManager = computerManager;
        }

        [Route("")]
        [HttpGet]
        public IEnumerable<ComputerDto> Get()
        {
            return this.computerManager.GetComputers();
        }

        [Route("{id:int}")]
        [HttpGet]
        public ComputerDto Get(int id)
        {
            return this.computerManager.GetComputer(id);
        }

        [Route("Search")]
        [HttpGet]
        public IEnumerable<ComputerDto> Search([FromUri] ComputerSearchRequest searchRequest)
        {
            return this.computerManager.SearchComputers(searchRequest);
        }
    }
}
