using System.Collections.Generic;
using Kawaii.NetworkDocumentation.AppDataService.ServiceModel;

namespace Kawaii.NetworkDocumentation.AppDataService.Managers
{
    public class CompanyManager : ICompanyManager
    {
        public IEnumerable<CompanyDto> GetCompanies()
        {
            var companyDtos = new List<CompanyDto>();

            return companyDtos;
        }
    }
}