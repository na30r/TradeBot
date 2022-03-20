using TradeBot.Models.Enum;

namespace TradeBot.Models.ViewModels
{
    public class SearchCandleViewModel
    {
        public CryptoType coinName { get; set; }
        public ResolutionType timeFrame { get; set; }
        public DateTime from { get; set; }
        public DateTime to { get; set; }
    }
}
