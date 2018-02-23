using System.Collections.Generic;
using System.Web.Http;
using Kawaii.NetworkDocumentation.AppDataService.Managers;
using Kawaii.NetworkDocumentation.AppDataService.ServiceModel;

namespace Kawaii.NetworkDocumentation.AppDataService.Controllers
{
    public class ComputersController : ApiController
    {
        private readonly IComputerManager computerManager;

        public ComputersController(IComputerManager computerManager)
        {
            this.computerManager = computerManager;
        }

        public IEnumerable<ComputerDto> GetAll()
        {
            return this.computerManager.GetComputers();
        }
    }
}
