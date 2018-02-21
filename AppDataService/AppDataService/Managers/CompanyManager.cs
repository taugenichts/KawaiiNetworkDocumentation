using System.Collections.Generic;
using Kawaii.NetworkDocumentation.AppDataService.DataModel.Database;
using Kawaii.NetworkDocumentation.AppDataService.ServiceModel;

namespace Kawaii.NetworkDocumentation.AppDataService.Managers
{
    public class CompanyManager : ICompanyManager
    {
        private readonly IDatabaseSession dbSession;

        public CompanyManager(IDatabaseSession dbSession)
        {
            this.dbSession = dbSession;
        }

        public IEnumerable<CompanyDto> GetCompanies()
        {
            var companyDtos = new List<CompanyDto>();

            return companyDtos;
        }
    }
}