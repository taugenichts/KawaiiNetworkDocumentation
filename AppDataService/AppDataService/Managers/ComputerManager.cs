using System;
using System.Collections.Generic;
using System.Linq;
using Kawaii.NetworkDocumentation.AppDataService.ServiceModel;
using Kawaii.NetworkDocumentation.AppDataService.DataModel;
using Kawaii.NetworkDocumentation.AppDataService.DataModel.Database;

namespace Kawaii.NetworkDocumentation.AppDataService.Managers
{
    public class ComputerManager : BaseManager, IComputerManager
    {
        public CreatedResponse CreateComputer(ComputerDto computer)
        {
            throw new NotImplementedException();
        }

        public DeletedResponse DeleteComputer(int id, DateTime lastModified)
        {
            throw new NotImplementedException();
        }

        public ComputerDto GetComputer(int Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ComputerDto> GetComputers(int takeFirst = 0)
        {
            var computers = new SqlSelect<Computer>().Run(this.DatabaseSession);

            return computers.Select(x => new ComputerDto
            {
                ComputerId = x.ComputerId,
                Inactive = x.Inactive,
                Name = x.Name,
                LastModified = x.LastModified,
                LastModifiedBy = x.LastModifiedBy,
                StaticIp = x.StaticIp
            });
        }

        public UpdatedResponse UpdateComputer(ComputerDto computer)
        {
            throw new NotImplementedException();
        }
    }
}