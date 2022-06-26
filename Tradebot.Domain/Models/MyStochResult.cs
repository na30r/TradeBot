using Skender.Stock.Indicators;

namespace TradeBot.Domain.Models
{
    public class MyStochResult : StochResult , ISignalable
    {
        public decimal? sma3 { get; set; }
        public bool longSignal { get; set; }
    }

    public interface ISignalable
    {
        bool longSignal { get; set; }
    }

    public class BaseResult : ResultBase { }

}
