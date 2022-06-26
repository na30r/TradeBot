using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeBot.Domain.Models;
using TradeBot.Domain.Models.Enum;

namespace WindowsServiceV1.Strategies
{
    public abstract class BaseStrategy : IBaseStrategy
    {
        public abstract List<CandlePosition> Strategy1Hour(List<CandlePosition> candles, CryptoType cryptoType);

        //protected readonly CandleRepository _candleRepository;
        //public BaseStrategy(CandleRepository candleRepository)
        //{
        //    _candleRepository = candleRepository;
        //}
        //public void Stoch30Min(CryptoType cryptoType)
        //{

        //}



    }
}
