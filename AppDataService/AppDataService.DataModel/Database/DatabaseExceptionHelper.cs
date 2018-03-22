using System;

namespace Kawaii.NetworkDocumentation.AppDataService.DataModel.Database
{
    public static class DatabaseExceptionHelper
    {
        public static void ThrowConcurrencyExcepiption(int id)
        {
            throw new Exception(string.Format("Concurrency exception for id: {0}", id));
        }
    }
}
