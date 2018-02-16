namespace Kawaii.NetworkDocumentation.AppDataService.DataModel
{
    public class Person : IDataModel
    {
        public int PersonId { get; internal set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Suffix { get; set; }

        public string Prefix { get; set; }

        public int? CompanyId { get; set; }

        public int Id
        {
            get { return this.PersonId; }
            set { this.PersonId = value; }
        }
    }
}
