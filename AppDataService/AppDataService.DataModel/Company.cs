namespace Kawaii.NetworkDocumentation.AppDataService.DataModel
{
    public class Company : IDataModel
    {
        public int CompanyId { get; internal set; }

        public string Name { get; set; }

        public string Street { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string AdditionalAddressLine1 { get; set; }

        public string AdditionalAddressLine2 { get; set; }

        public string WebsiteUrl { get; set; }

        public string EmailAddress { get; set; }

        public int Id
        {
            get { return this.CompanyId; }
            set { this.CompanyId = value; }
        }
    }
}
