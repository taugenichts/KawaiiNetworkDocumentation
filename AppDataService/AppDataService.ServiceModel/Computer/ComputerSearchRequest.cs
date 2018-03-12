using System.Runtime.Serialization;

namespace Kawaii.NetworkDocumentation.AppDataService.ServiceModel.Computer
{
    [DataContract]
    public class ComputerSearchRequest
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string StaticIp { get; set; }
    }
}
