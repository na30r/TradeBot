using TradeBot.Domain.Models;
using TradeBot.Domain.Models.Enum;

namespace WindowsServiceV1.Strategies
{
    public interface IBaseStrategy
    {
        public List<CandlePosition> Strategy1Hour(List<CandlePosition> candles, CryptoType cryptoType);

    }
}