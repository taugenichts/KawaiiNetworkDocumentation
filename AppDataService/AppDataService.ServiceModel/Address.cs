using System.Runtime.Serialization;

namespace Kawaii.NetworkDocumentation.AppDataService.ServiceModel
{
    [DataContract]
    public class Address
    {
        [DataMember]
        public string Street { get; set; }

        [DataMember]
        public string PostalCode { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public string AdditionalAddressLine1 { get; set; }

        [DataMember]
        public string AdditionalAddressLine2 { get; set; }

        [DataMember]
        public string WebsiteUrl { get; set; }

        [DataMember]
        public string EmailAddress { get; set; }
    }
}
