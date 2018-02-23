using Kawaii.NetworkDocumentation.AppDataService.DataModel.Database;
using Unity.Attributes;

namespace Kawaii.NetworkDocumentation.AppDataService.Managers
{
    public abstract class BaseManager
    {
        [Dependency]
        protected IDatabaseSession DatabaseSession { get; set; }
    }
}