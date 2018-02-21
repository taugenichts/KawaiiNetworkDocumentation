using System.Data;

namespace Kawaii.NetworkDocumentation.AppDataService.DataModel.Database
{
    public interface IDatabaseSession
    {
        IDbConnection GetConnection();
    }
}
