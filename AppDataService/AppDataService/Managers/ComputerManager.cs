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

        public ComputerDto GetComputer(int id)
        {
            var computer = new SqlSelect<Computer>()
                .AddCondition(new SqlConditionGroup
                {
                    ChildConditions = new SqlCondition[]
                    {
                        new SqlCondition("ComputerId", ComparisionOperator.Equals, id),
                        new SqlCondition("ComputerId", ComparisionOperator.Equals, id, LogicalOperator.And)
                    }
                })
                .Run(this.DatabaseSession)
                .Single();

            return ToComputerDto(computer);
        }

        public IEnumerable<ComputerDto> GetComputers(int takeFirst = 0)
        {
            var computers = new SqlSelect<Computer>().Run(this.DatabaseSession);
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