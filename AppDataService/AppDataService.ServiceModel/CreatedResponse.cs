using System;

namespace Kawaii.NetworkDocumentation.AppDataService.ServiceModel
{
    public class CreatedResponse
    {
        public int ClientId { get; set; }

        public int ServerId { get; set; }

        public DateTime LastModified { get; set; }
    }
}
