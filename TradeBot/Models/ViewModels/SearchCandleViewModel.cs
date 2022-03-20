using TradeBot.Models.Enum;

namespace TradeBot.Models.ViewModels
{
    public class SearchCandleViewModel
    {
        public int coinName { get; set; }
        public int timeFrame { get; set; }
        public string from { get; set; }
        public string to { get; set; }
    }
}
