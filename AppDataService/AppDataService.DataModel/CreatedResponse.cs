﻿using System;

namespace Kawaii.NetworkDocumentation.AppDataService.DataModel
{
    public class CreatedResponse : IRecordChangeInfo
    {
        public int ClientId { get; set; }

        public int ServerId { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
