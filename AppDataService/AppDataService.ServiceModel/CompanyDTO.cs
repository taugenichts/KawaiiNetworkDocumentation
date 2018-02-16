using System.Runtime.Serialization;

namespace Kawaii.NetworkDocumentation.AppDataService.ServiceModel
{
    [DataContract]
    public partial class CompanyDto
    {
        [DataMember(IsRequired = true)]
        public int CompanyId { get; internal set; }

        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        [DataMember]
        public Address Address { get; set; }
    }
}
