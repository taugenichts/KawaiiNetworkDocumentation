using System.Collections.Generic;
using System.Web.Http;
using Kawaii.NetworkDocumentation.AppDataService.DataModel;
using Kawaii.NetworkDocumentation.AppDataService.Managers;
using Kawaii.NetworkDocumentation.AppDataService.ServiceModel.Computer;

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

        [Route("")]
        [HttpPost]
        public CreatedResponse Create(ComputerDto computer)
        {
            return this.computerManager.CreateComputer(computer);
        }

        [Route("")]
        [HttpPut]
        public UpdatedResponse Update(ComputerDto computer)
        {
            return this.computerManager.UpdateComputer(computer);
        }

        [Route("")]
        [HttpDelete]
        public DeletedResponse Delete(DeleteRequest deleteRequest)
        {
            return this.computerManager.DeleteComputer(deleteRequest);
        }
    }
}
