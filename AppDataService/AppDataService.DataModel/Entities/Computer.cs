using System;
using Kawaii.NetworkDocumentation.AppDataService.DataModel;

namespace Kawaii.NetworkDocumentation.AppDataService.DataModel.Entities
{
    public class Computer : IDataModel, IRecordChangeInfo
    {
        public int Id { get => this.ComputerId; set => this.ComputerId = value; }

        public int ComputerId { get; set; }

        public string Name { get; set; }

        public string StaticIp { get; set; }

        public bool Inactive { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
