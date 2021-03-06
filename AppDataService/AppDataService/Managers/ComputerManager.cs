﻿using System;
using System.Collections.Generic;
using System.Linq;
using Kawaii.NetworkDocumentation.AppDataService.DataModel;
using Kawaii.NetworkDocumentation.AppDataService.DataModel.Entities;
using Kawaii.NetworkDocumentation.AppDataService.DataModel.Database;
using Kawaii.NetworkDocumentation.AppDataService.ServiceModel.Computer;

namespace Kawaii.NetworkDocumentation.AppDataService.Managers
{
    public class ComputerManager : BaseManager, IComputerManager
    {
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

        public CreatedResponse CreateComputer(ComputerDto computer)
        {
            var response = new SqlInsert<Computer>(ToComputer(computer)).Run(this.DatabaseSession);
            return response;
        }

        public UpdatedResponse UpdateComputer(ComputerDto computer)
        {
            var response = new SqlUpdate<Computer>(ToComputer(computer)).Run(this.DatabaseSession);
            return response;
        }

        public DeletedResponse DeleteComputer(DeleteRequest request)
        {
            var response = new SqlDelete<Computer>(ToComputer(request)).Run(this.DatabaseSession);
            return response;
        }

        private static ComputerDto ToComputerDto(Computer computer)
        {
            return new ComputerDto
            {
                ComputerId = computer.ComputerId,
                Inactive = computer.Inactive,
                Name = computer.Name,
                StaticIp = computer.StaticIp,
                RowVersion = computer.RowVersion
            };
        }

        private static Computer ToComputer(ComputerDto computer)
        {
            return new Computer
            {
                ComputerId = computer.ComputerId,
                Inactive = computer.Inactive,
                Name = computer.Name,
                StaticIp = computer.StaticIp,
                RowVersion = computer.RowVersion
            };
        }

        private static Computer ToComputer(DeleteRequest deleteRequest)
        {
            return new Computer
            {
                ComputerId = deleteRequest.Id,
                RowVersion = deleteRequest.RowVersion
            };
        }
    }
}