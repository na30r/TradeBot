using System.ComponentModel;
using System.Reflection;

namespace WindowsServiceV1
{
    public static class Helper
    {
        public static string GetDescription(this Enum value)
        {
            return ((DescriptionAttribute)Attribute.GetCustomAttribute(
                value.GetType().GetFields(BindingFlags.Public | BindingFlags.Static)
                    .Single(x => x.GetValue(null).Equals(value)),
                typeof(DescriptionAttribute)))?.Description ?? value.ToString();
        }
        public static DateTime UnixTimeStampToDateTime(this int unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
        public static void LogTofile(string logString)
        {
            File.AppendAllLines(@"C:\myService\mynote3.txt", new List<string> { logString });
        }

        public static string GetConnectionString()
        {
            return "Server=.;Database=TradeBot;User Id=service;Password=1qaz!QAZ;";
        }

        public static string GetConnectionStringWithoutPass()
        {
            return "Server=.;Database=TradeBot;User Id=service;Password=1qaz!QAZ;";
        }

    }



}
