using Skender.Stock.Indicators;

namespace TradeBot.Domain.Models
{
    public class MyStochResult : StochResult
    {
        public decimal? sma3 { get; set; }
        public bool longSignal { get; set; }


    }
}
