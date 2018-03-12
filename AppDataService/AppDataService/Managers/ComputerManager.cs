using System;
using System.Collections.Generic;
using System.Linq;
using Kawaii.NetworkDocumentation.AppDataService.ServiceModel;
using Kawaii.NetworkDocumentation.AppDataService.ServiceModel.Computer;
using Kawaii.NetworkDocumentation.AppDataService.DataModel.Entities;
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

        public ComputerDto GetComputer(int id)
        {
            var computer = new SqlSelect<Computer>()
                .AddCondition("ComputerId", ComparisionOperator.Equals, id)
                .Run(this.DatabaseSession)
                .Single();

            return ToComputerDto(computer);
        }

        public IEnumerable<ComputerDto> GetComputers(int takeFirst = 0)
        {
            var computers = new SqlSelect<Computer>()
                            .First(takeFirst)
                            .Run(this.DatabaseSession);

            return computers.Select(ToComputerDto);
        }

        public IEnumerable<ComputerDto> SearchComputers(ComputerSearchRequest searchRequest)
        {
            var computers = new SqlSelect<Computer>()
                                .AddCondition("Name", ComparisionOperator.Like, searchRequest.Name)
                                .AddCondition("StaticIp", ComparisionOperator.Like, searchRequest.StaticIp, LogicalOperator.And)
                                .Run(this.DatabaseSession);

            return computers.Select(ToComputerDto);
        }

        public UpdatedResponse UpdateComputer(ComputerDto computer)
        {
            throw new NotImplementedException();
        }

        private static ComputerDto ToComputerDto(Computer computer)
        {
            return new ComputerDto
            {
                ComputerId = computer.ComputerId,
                Inactive = computer.Inactive,
                Name = computer.Name,
                LastModified = computer.LastModified,
                LastModifiedBy = computer.LastModifiedBy,
                StaticIp = computer.StaticIp
            };
        }
    }
}