using System.Collections.Generic;
using Kawaii.NetworkDocumentation.AppDataService.ServiceModel;


namespace Kawaii.NetworkDocumentation.AppDataService.Managers
{
    public interface ICompanyManager
    {
        IEnumerable<CompanyDto> GetCompanies();
    }
}
