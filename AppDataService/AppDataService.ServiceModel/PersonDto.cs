using System.Runtime.Serialization;

namespace Kawaii.NetworkDocumentation.AppDataService.ServiceModel
{
    [DataContract]
    public partial class PersonDto
    {
        [DataMember(IsRequired = true)]
        public int PersonId { get; internal set; }

        [DataMember(IsRequired = true)]
        public string FirstName { get; set; }

        [DataMember]
        public string MiddleName { get; set; }

        [DataMember(IsRequired = true)]
        public string LastName { get; set; }

        [DataMember]
        public string Suffix { get; set; }

        [DataMember]
        public string Prefix { get; set; }

        [DataMember]
        public int? CompanyId { get; set; }

        [DataMember]
        public string CompanyName { get; set; }
    }
}
