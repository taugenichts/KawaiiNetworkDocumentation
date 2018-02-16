using System.Collections.Generic;
using System.Web.Http;
using Kawaii.NetworkDocumentation.AppDataService.Managers;
using Kawaii.NetworkDocumentation.AppDataService.ServiceModel;

namespace Kawaii.NetworkDocumentation.AppDataService.Controllers
{
    public class CompaniesController : ApiController
    {
        private readonly ICompanyManager companyManager;

        public CompaniesController(ICompanyManager companyManager)
        {
            this.companyManager = companyManager;
        }

        [HttpGet]
        public IEnumerable<CompanyDto> GetAllCompanies()
        {
            return this.companyManager.GetCompanies();
        }
    }
}
