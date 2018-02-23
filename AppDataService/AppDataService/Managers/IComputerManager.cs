using System;
using System.Collections.Generic;
using Kawaii.NetworkDocumentation.AppDataService.ServiceModel;

namespace Kawaii.NetworkDocumentation.AppDataService.Managers
{
    public interface IComputerManager
    {
        IEnumerable<ComputerDto> GetComputers(int takeFirst = 0);

        ComputerDto GetComputer(int Id);

        CreatedResponse CreateComputer(ComputerDto computer);

        UpdatedResponse UpdateComputer(ComputerDto computer);

        DeletedResponse DeleteComputer(int id, DateTime lastModified);
    }
}
