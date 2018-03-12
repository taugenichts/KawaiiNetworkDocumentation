using System.Runtime.Serialization;

namespace Kawaii.NetworkDocumentation.AppDataService.ServiceModel
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
