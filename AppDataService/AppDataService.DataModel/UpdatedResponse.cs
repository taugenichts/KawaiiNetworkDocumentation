using System;

namespace Kawaii.NetworkDocumentation.AppDataService.DataModel
{
    public class UpdatedResponse : IRecordChangeInfo
    {
        public int ServerId { get; set; }

        public DateTime LastModified { get; set; }

        public string LastModifiedBy { get; set; }
    }
}
