using System;
using System.Linq;

namespace Kawaii.NetworkDocumentation.AppDataService.Tests.TestHelpers
{
    public static class Randomize
    {
        private static Random random = new Random();

        public static string String(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static int Int(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue);
        }

        public static bool Bool()
        {
            return random.Next(0, 1) == 1;
        }

        public static DateTime Date(int daysBeforeNow, int daysAfterNow)
        {
            int daysOffset = random.Next(daysBeforeNow, daysAfterNow);
            return DateTime.Now.AddDays(daysOffset);
        }
    }
}
