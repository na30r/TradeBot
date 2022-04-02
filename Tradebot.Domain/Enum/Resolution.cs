namespace TradeBot.Domain.Models.Enum
{
    public static class Resolution
    {
        public static readonly Dictionary<ResolutionType, string> Resolutions = new Dictionary<ResolutionType, string>()
        {
            {ResolutionType.Monthly, "M" },
            {ResolutionType.Weekly, "W" },
            {ResolutionType.Daily, "D" },
            {ResolutionType._1h, "60" },
            {ResolutionType._30min, "30" },
            {ResolutionType._15min, "15" },
            {ResolutionType._5min, "5" },
            {ResolutionType._1min, "1" }

        };
        public static readonly Dictionary<ResolutionType, int> ResolutionsInMinute = new Dictionary<ResolutionType, int>()
        {
            {ResolutionType.Monthly, 60*24*30 },
            {ResolutionType.Weekly, 60*24*7 },
            {ResolutionType.Daily, 60*24 },
            {ResolutionType._1h, 60 },
            {ResolutionType._30min, 30 },
            {ResolutionType._15min, 15 },
            {ResolutionType._5min, 5 },
            {ResolutionType._1min, 1 }

        };

        public static readonly Dictionary<ResolutionType, int> ResolutionTimeDifference = new Dictionary<ResolutionType, int>()
        {
            {ResolutionType.Monthly, 1 },
            {ResolutionType.Weekly,  604800},
            {ResolutionType.Daily, 86400 },
            {ResolutionType._1h, 3600 },
            {ResolutionType._30min, 1800 },
            {ResolutionType._15min, 900 },
            {ResolutionType._5min, 300},
            {ResolutionType._1min, 60 },
            {ResolutionType._4h, 14400 }
        };
    }
    public enum ResolutionType
    {
        Monthly = 1,
        Weekly = 2,
        Daily = 3,
        _1h = 4,
        _30min = 5,
        _15min = 9,
        _5min = 7,
        _1min = 8,
        _4h = 10
    }
}
