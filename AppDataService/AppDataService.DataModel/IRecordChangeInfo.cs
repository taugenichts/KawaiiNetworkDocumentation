using System;

namespace Kawaii.NetworkDocumentation.AppDataService.DataModel
{
    public interface IRecordChangeInfo
    {
        byte[] RowVersion { get; set; }
    }
}
