using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeBot.Domain.Models;
using TradeBot.Domain.Models.Enum;

namespace WindowsServiceV1.Strategies
{
    public class Strategytester
    {
        public void strategyTest<T>(CryptoType cryptoType, List<Candle> candles, List<Func<List<CandlePosition>, List<CandlePosition>>> takeProfits,
            List<Func<List<CandlePosition>, List<CandlePosition>>> stopLimits)
        {
            List<CandlePosition> candlePositions = candles.Select(a => new CandlePosition(a)).ToList();
            var newInstance = Activator.CreateInstance(typeof(T));
            var baseStrategy = newInstance as BaseStrategy;
            var s = baseStrategy.Strategy1Hour(candlePositions, cryptoType);
            foreach (var item in takeProfits)
            {
               var res = item.Invoke(s);
            }

        }
        public void BuyPosition(List<Candle> candles, long buyPrice, Strategy sellStrategy, Strategy stopLossStrategy, bool isDynamic)
        {



        }
    }
}
