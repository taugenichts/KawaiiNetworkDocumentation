using System.Collections.Generic;
using Kawaii.NetworkDocumentation.AppDataService.ServiceModel;

namespace Kawaii.NetworkDocumentation.AppDataService.Managers
{
    public class PersonManager : IPersonManager
    {
        public IEnumerable<PersonDto> GetPersons()
        {
            var personDtos = new List<PersonDto>();

            return personDtos;
        }
    }
}