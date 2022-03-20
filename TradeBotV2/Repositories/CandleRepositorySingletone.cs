using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TradeBot.Models;
using TradeBot.Models.Enum;
using TradeBot.Repositories;

namespace TradeBotV2.Repositories
{
    public class CandleRepositorySingletone
    {
        private ApplicationDbContext _context;

        public CandleRepositorySingletone()
        {
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
            List<Candle> insertCandles = new List<Candle>();
            foreach (var candle in candles)
            {
                if (IsExisted(candle) == false)
                {
                    insertCandles.Add(candle);
                }
            }
            try
            {
                _context.AddRange(insertCandles);
                return _context.SaveChanges();
            }
            catch (Exception e)
            {
                var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionBuilder.UseSqlServer("Server=.; Initial Catalog=TradeBot; Integrated Security=True");
                using ApplicationDbContext ctx = new ApplicationDbContext(optionBuilder.Options);
                ctx.AddRange(insertCandles);
                return ctx.SaveChanges();
            }

        }



    }
}
