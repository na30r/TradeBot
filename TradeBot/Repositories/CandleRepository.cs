using TradeBot.Models;
using TradeBot.Models.Enum;

namespace TradeBot.Repositories
{
    public class CandleRepository
    {
        private readonly ApplicationDbContext _context;

        public CandleRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public bool IsExisted(Candle candle)
        {
            return _context.Candles.Any(a => a.DateTime == candle.DateTime && a.CoinName == candle.CoinName);
        }

        public int InsertCandle(Candle candle)
        {
            if (IsExisted(candle) == false)
            {
                _context.Add(candle);
                return _context.SaveChanges();
            }
            return 0;
        }

        public int InsertCandles(List<Candle> candles)
        {
            foreach (var candle in candles)
            {
                if (IsExisted(candle) == false)
                {
                    _context.Add(candle);
                }
            }
            return _context.SaveChanges();
        }

        public List<Candle> GetCandles(CryptoType cryptoType,ResolutionType resolution , DateTime from , DateTime to)
        {
           var candles = _context.Candles
                .Where(a=>a.DateTime > from && a.DateTime<to && a.CoinName == (int) cryptoType)
                .Where(a=>a.Time % Resolution.ResolutionTimeDifference[resolution] == 0)
                .ToList();
            return candles;
        }

    

    }
}
