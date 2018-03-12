using System;
using System.Collections.Generic;
using Kawaii.NetworkDocumentation.AppDataService.ServiceModel;
using Kawaii.NetworkDocumentation.AppDataService.ServiceModel.Computer;

namespace Kawaii.NetworkDocumentation.AppDataService.Managers
{
    public interface IComputerManager
    {
        IEnumerable<ComputerDto> GetComputers(int takeFirst = 0);

        IEnumerable<ComputerDto> SearchComputers(ComputerSearchRequest searchRequest);

        ComputerDto GetComputer(int Id);

        CreatedResponse CreateComputer(ComputerDto computer);

        UpdatedResponse UpdateComputer(ComputerDto computer);

        DeletedResponse DeleteComputer(int id, DateTime lastModified);
    }
}
