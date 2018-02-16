using System.Collections.Generic;
using System.Web.Http;
using Kawaii.NetworkDocumentation.AppDataService.Managers;
using Kawaii.NetworkDocumentation.AppDataService.ServiceModel;

namespace Kawaii.NetworkDocumentation.AppDataService.Controllers
{
    public class PersonsController : ApiController
    {
        private readonly IPersonManager personManager;

        public PersonsController(IPersonManager personManager)
        {
            this.personManager = personManager;
        }

        [HttpGet]
        public IEnumerable<PersonDto> GetAllPersons()
        {
            return this.personManager.GetPersons();
        }
    }
}
