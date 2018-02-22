using System.Collections.Generic;
using System.Data;
using System.Linq;
using Kawaii.NetworkDocumentation.AppDataService.DataModel;
using Kawaii.NetworkDocumentation.AppDataService.DataModel.Database;
using Kawaii.NetworkDocumentation.AppDataService.ServiceModel;

namespace Kawaii.NetworkDocumentation.AppDataService.Managers
{
    public class PersonManager : IPersonManager
    {
        private readonly IDatabaseSession dbSession;

        public PersonManager(IDatabaseSession dbSession)
        {
            this.dbSession = dbSession;
        }

        public IEnumerable<PersonDto> GetPersons()
        {
            var persons = new SqlSelect<Person>().Run(this.dbSession);

            return persons.Select(x => new PersonDto(x.PersonId)
                                        {
                                            FirstName = x.FirstName,
                                            LastName = x.LastName,
                                            MiddleName = x.MiddleName,
                                            CompanyId = x.CompanyId,
                                            Suffix = x.Suffix,
                                            Prefix = x.Prefix
                                        });
        }
    }
}