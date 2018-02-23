using System;

namespace Kawaii.NetworkDocumentation.AppDataService.DataModel
{
    public interface IDataModel
    {
        int Id { get; set; }

        DateTime LastModified { get; set; }

         string LastModifiedBy { get; set; }
    }
}
