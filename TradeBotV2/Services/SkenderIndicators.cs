using Skender.Stock.Indicators;
using TradeBot.Models;
using TradeBot.Models.Enum;
using TradeBot.Repositories;
using TradeBotV2.Models;

namespace TradeBot.Services
{
    public class SkenderIndicators
    {

        private readonly ApplicationDbContext _context;
        private readonly CandleRepository _candleRepository;
        public GhasedakSMSProvider _SMSProvider;

        public SkenderIndicators(ApplicationDbContext context, CandleRepository candleRepository, GhasedakSMSProvider ghasedakSMSProvider)
        {
            _context = context;
            _candleRepository = candleRepository;
            _SMSProvider = ghasedakSMSProvider;
        }

        public void StochStrategy4h(CryptoType cryptoType, DateTime from, DateTime to)
        {
            StochStrategy(cryptoType, ResolutionType._4h, from, to);
        }
        public void StochStrategy30min(CryptoType cryptoType, DateTime from, DateTime to)
        {
            StochStrategy(cryptoType, ResolutionType._30min, from, to);
        }
        public void StochStrategyDaily(CryptoType cryptoType, DateTime from, DateTime to)
        {
            StochStrategy(cryptoType, ResolutionType.Daily, from, to);
        } 
       
        private void StochStrategy(CryptoType cryptoType, ResolutionType resolution, DateTime from, DateTime to)
        {
            IEnumerable<Quote> quotes = _candleRepository.GetCandles(cryptoType,resolution,from,to).Select(a=>(Quote)a).ToList();

            // calculate 20-period SMA
            //IEnumerable<SmaResult> results = quotes.GetStoch(lookbackPeriods, signalPeriods, smoothPeriods); ;

            var myStochList = quotes.GetStoch(14,3,7).ToList().AddSmaToStoch(3);

            // use results as needed for your use case (example only)
            for (int i = 1; i < myStochList.Count(); i++)
            {
                if (myStochList[i].K > 15 && myStochList[i-1].K < 15 && myStochList[i].K> myStochList[i].sma3)
                {
                    myStochList[i].longSignal = true;
                }
            }

            if (myStochList.LastOrDefault().longSignal == true)
            {
                _SMSProvider.Alert("هشدار خرید ");
            }
            var longSignals = myStochList.Where(a => a.longSignal).ToList();
            foreach (MyStochResult r in longSignals)
            {
                    Console.WriteLine($"SMA on {r.Date} was k: {r.K:N4} D:{r.D:N4} Oscillator:{r.Oscillator:N4}  signal:{r.Signal:N4} 3sma:{r.sma3:N4} longSignal:{r.longSignal:N4}  ");
            }
            //k abie d ghermeze 

            //k>15
            //k> miangin
        }

       
    }
    public static class IndicatorsExtension
    {
        public static List<MyStochResult> AddSmaToStoch(this List<StochResult> stochResults, int lookbackPeriods)
        {
            var rr = stochResults.Select(a =>
           new Quote()
           {
               Close = a.D ?? 0,
               Date = a.Date,
               High = a.D ?? 0,
               Low = a.D ?? 0,
               Open = a.D ?? 0,
               Volume = a.D ?? 0,
           }).ToList();
            var sma = rr.GetSma(lookbackPeriods).ToList();
            var myStochList = new List<MyStochResult>();
            for (int i = 0; i < stochResults.Count; i++)
            {
                var myStochItem = new MyStochResult();
                myStochItem.Oscillator = stochResults[i].K;
                myStochItem.Signal = stochResults[i].D;
                myStochItem.PercentJ = stochResults[i].J;
                myStochItem.Date = stochResults[i].Date;
                myStochItem.longSignal = false;
                myStochItem.sma3 = sma[i].Sma;
                myStochList.Add(myStochItem);
            }
            return myStochList;
        }
    }
}
