using System;

namespace Kawaii.NetworkDocumentation.AppDataService.DataModel
{
    public interface IRecordChangeInfo
    {
        DateTime LastModified { get; set; }

        string LastModifiedBy { get; set; }
    }
}
