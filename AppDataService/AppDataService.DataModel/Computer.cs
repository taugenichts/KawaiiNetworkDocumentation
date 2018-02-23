using System;

namespace Kawaii.NetworkDocumentation.AppDataService.DataModel
{
    public class Computer : IDataModel
    {
        public int Id { get => this.ComputerId; set => this.ComputerId = value; }

        public int ComputerId { get; set; }

        public string Name { get; set; }

        public string StaticIp { get; set; }

        public bool Inactive { get; set; }

        public DateTime LastModified { get; set; }

        public string LastModifiedBy { get; set; }
    }
}
