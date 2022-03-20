using Skender.Stock.Indicators;
using TradeBot.Models;
using TradeBot.Models.Enum;
using TradeBot.Repositories;

namespace TradeBot.Services
{
    public class SkenderIndicators
    {

        private readonly ApplicationDbContext _context;
        private readonly CandleRepository _candleRepository;

        public SkenderIndicators(ApplicationDbContext context, CandleRepository candleRepository)
        {
            _context = context;
            _candleRepository = candleRepository;
        }

        public void test(CryptoType cryptoType, ResolutionType resolution, DateTime from, DateTime to)
        {
            IEnumerable<Quote> quotes = _candleRepository.GetCandles(cryptoType,resolution,from,to).Select(a=>(Quote)a).ToList();

            // calculate 20-period SMA
            //IEnumerable<SmaResult> results = quotes.GetStoch(lookbackPeriods, signalPeriods, smoothPeriods); ;
            var results = quotes.GetStoch(14,3,7);

            var rr = results.Select(a =>
            new Quote()
            {
                Close = a.D ?? 0 ,
                Date = a.Date ,
                High =  a.D ?? 0,
                Low = a.D ?? 0,
                Open = a.D ?? 0,
                Volume = a.D ?? 0,
            }).ToList();
            var sma = rr.GetSma(3).ToList();
            // use results as needed for your use case (example only)
            foreach (StochResult r in results)
            {
                Console.WriteLine($"SMA on {r.Date} was k: {r.K:N4} D:{r.D:N4} Oscillator:{r.Oscillator:N4}  signal:{r.Signal:N4} ");
            }
            for (int i = rr.Count-10; i < rr.Count; i++)
            {
                Console.WriteLine($"miangin on {sma[i].Date} was miangin: {sma[i].Sma:N4}");
            }
            //k abie d ghermeze 
        }
    }
}
