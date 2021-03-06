﻿using System;
using System.Runtime.Serialization;

namespace Kawaii.NetworkDocumentation.AppDataService.ServiceModel.Computer
{
    [DataContract]
    public class ComputerDto
    {        
        [DataMember(IsRequired = true)]
        public int ComputerId { get; set; }

        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        [DataMember]
        public string StaticIp { get; set; }

        [DataMember]
        public bool Inactive { get; set; }

        [DataMember(IsRequired = true)]
        public byte[] RowVersion { get; set; }
    }
}
