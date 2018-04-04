using System;

namespace Kawaii.NetworkDocumentation.AppDataService.DataModel
{
    public class UpdatedResponse : IRecordChangeInfo
    {
        public int ServerId { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
