using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using TradeBot.Domain.Models;
using TradeBot.Domain.Models.Enum;
using TradeBot.Domain;
using static System.Net.Mime.MediaTypeNames;

namespace WindowsServiceV1
{
    public class CandleRepository
    {
        private ApplicationDbContext _context;

        public CandleRepository(ApplicationDbContext context)
        {
            _context = context ;
        }

        public bool IsExistedAndApproved(Candle candle, List<Candle> databaseCandles = null)
        {
            var res = new Candle();
            try
            {
                if (databaseCandles != null && databaseCandles.Count > 0)
                {
                    res = databaseCandles.FirstOrDefault(a => a.DateTime == candle.DateTime && a.CoinName == candle.CoinName);
                }
                else
                {
                    res = _context.Candles.FirstOrDefault(a => a.DateTime == candle.DateTime && a.CoinName == candle.CoinName);
                }
                if (res == null) { return false; }
                else if (res.Approved == true)
                {
                    return true;
                }
                else if (res.Approved == false)
                {
                    _context.Remove(res);
                    _context.SaveChanges();
                    return false;
                }
            }
            catch (Exception e)
            {
                var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionBuilder.UseSqlServer("Server=.; Initial Catalog=TradeBot; Integrated Security=True");
                using ApplicationDbContext ctx = new ApplicationDbContext(optionBuilder.Options);
                return ctx.Candles.Any(a => a.DateTime == candle.DateTime && a.CoinName == candle.CoinName);
            }
            throw new Exception();
        }

        internal List<Candle> GetNotApprovedList(CryptoType cryptoName, ResolutionType resolution)
        {
            try
            {
                var candle = _context.Candles.Where(a => a.CoinName == (int)cryptoName && a.Timeframe == resolution)
                    .Where(a => !a.Approved)
                    .OrderByDescending(a => a.DateTime).ToList();
                return candle;

            }
            catch (Exception e)
            {
                Helper.LogTofile("error from GetNotExpiredList " + e.Message);
                throw;
            }
        }

        public int InsertCandle(Candle candle)
        {

            var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionBuilder.UseSqlServer("Server=.; Initial Catalog=TradeBot; Integrated Security=True", builder => builder.EnableRetryOnFailure());
            using ApplicationDbContext ctx = new ApplicationDbContext(optionBuilder.Options);
            if (IsExistedAndApproved(candle) == false)
            {
                _context.Add(candle);
                return _context.SaveChanges();
            }
            return 0;
        }

        public int InsertCandles(List<Candle> candles, ResolutionType resolution)
        {
            List<Candle> newCandles = new List<Candle>();
            List<Candle> databaseList = new List<Candle>();
            if (candles.Count > 0)
            {
                if (candles.FirstOrDefault() != null)
                {
                    databaseList = GetListOfCandles(candles, candles.FirstOrDefault().CoinName, resolution);
                }

            }

            foreach (var candle in candles)
            {
                if (IsExistedAndApproved(candle, databaseList) == false)
                {
                    newCandles.Add(candle);
                }
            }
            try
            {
                Helper.LogTofile($"save candles { candles.FirstOrDefault()?.CoinName }");
                _context.AddRange(newCandles);
                return _context.SaveChanges();
            }
            catch (Exception e)
            {
                File.AppendAllLines(@"C:\myService\mynote3.txt", new List<string> { $" {e.Message} + in save candles " });
                var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionBuilder.UseSqlServer("Server=.; Initial Catalog=TradeBot; Integrated Security=True");
                using ApplicationDbContext ctx = new ApplicationDbContext(optionBuilder.Options);
                ctx.AddRange(newCandles);
                return ctx.SaveChanges();
            }

        }

        private List<Candle> GetListOfCandles(List<Candle> candles, int coinName, ResolutionType resolution)
        {
            var datetimes = candles.Select(c => c.DateTime).ToList();
            var mylist = _context.Candles
                   .Where(a => a.CoinName == coinName)
                   .Where(a => datetimes.Contains(a.DateTime))
                   .Where(a => a.Timeframe == resolution)
                   .ToList();
            return mylist;
        }

        public List<Candle> GetCandles(CryptoType cryptoType, ResolutionType resolution, DateTime from, DateTime to)
        {
            var candles = _context.Candles
                .Where(a => a.DateTime > from && a.DateTime < to && a.CoinName == (int)cryptoType && a.Timeframe == resolution)
                // .Where(a => a.Time % Resolution.ResolutionTimeDifference[resolution] == 0)
                .ToList();
            return candles;
        }
        public bool IsSync(Candle lastCandle)
        {
            if (DateTime.Now.Subtract(lastCandle.DateTime) < TimeSpan.FromMinutes(30))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public Candle GetLastApprovedCandle(CryptoType cryptoName, ResolutionType resolution)
        {
            try
            {
                var candle = _context.Candles.Where(a => a.CoinName == (int)cryptoName && a.Approved && a.Timeframe == resolution)
                    .OrderByDescending(a => a.DateTime).FirstOrDefault();
                return candle;

            }
            catch (Exception e)
            {
                Helper.LogTofile("error from get last candle " + e.Message);
                throw;
            }
        }
        public Candle GetLastCandle(CryptoType cryptoName, ResolutionType resolution)
        {
            try
            {
                var candle = _context.Candles.Where(a => a.CoinName == (int)cryptoName && a.Timeframe == resolution)
                    .OrderByDescending(a => a.DateTime).FirstOrDefault();
                return candle;

            }
            catch (Exception e)
            {
                Helper.LogTofile("error from get last candle " + e.Message);
                throw;
            }
        }

        public void Dispose()
        {

        }
    }
}
