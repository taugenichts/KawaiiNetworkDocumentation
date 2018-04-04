using System;

namespace Kawaii.NetworkDocumentation.AppDataService.DataModel
{
    public class DeleteRequest : IRecordChangeInfo
    {
        public int Id { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
