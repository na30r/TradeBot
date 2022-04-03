using Skender.Stock.Indicators;
using TradeBot.Domain.Models;
using TradeBot.Domain.Models.Enum;

namespace WindowsServiceV1.Services
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
        public void StochStrategy1Hour(CryptoType cryptoType, DateTime from, DateTime to)
        {
            StochStrategy(cryptoType, ResolutionType._1h, from, to);
        }
        public void StochStrategyDaily(CryptoType cryptoType, DateTime from, DateTime to)
        {
            StochStrategy(cryptoType, ResolutionType.Daily, from, to);
        }

        private void StochStrategy(CryptoType cryptoType, ResolutionType resolution, DateTime from, DateTime to)
        {
            Helper.LogTofile($"enter stock cryptotype: {cryptoType} resolution: {resolution} from: {from}");
            IEnumerable<Quote> quotes = _candleRepository.GetCandles(cryptoType, resolution, from, to).Select(a => (Quote)a).ToList();

            // calculate 20-period SMA
            //IEnumerable<SmaResult> results = quotes.GetStoch(lookbackPeriods, signalPeriods, smoothPeriods); ;

            var stochResults = quotes.GetStoch(14, 3, 7).ToList();

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
            var sma = rr.GetSma(3).ToList();
            var quotesList = quotes.ToList();
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
            if (stochResults.Count > 1)
            {
                var res = myStochList.SkipLast(1).LastOrDefault();
                var str = $"{res.Date} k: {res.K:N4} D:{res.D:N4} Oscillator:{res.Oscillator:N4}  signal:{res.Signal:N4} 3sma:{res.sma3:N4} longSignal:{res.longSignal:N4} ";
                Helper.LogTofile(str);
                Console.WriteLine(str);
            }
            // use results as needed for your use case (example only)
            for (int i = 1; i < myStochList.Count(); i++)
            {
                if (myStochList[i].K > 15 && myStochList[i - 1].K < 15 && myStochList[i].K > myStochList[i].sma3)
                {
                    myStochList[i].longSignal = true;
                }
            }
            var lastCandle = myStochList.TakeLast(2).FirstOrDefault();
            if (lastCandle != null && lastCandle.longSignal == true)
            {
                _SMSProvider.AlertString($"هشدار خرید + {cryptoType} در {resolution}");
            }
            var longSignals = myStochList.Where(a => a.longSignal).ToList();
            foreach (MyStochResult r in longSignals)
            {
                var str = $"=>{cryptoType} in timeframe {resolution}  SMA on {r.Date}  k: {r.K:N4} D:{r.D:N4} 3sma:{r.sma3:N4} longSignal:{r.longSignal:N4} ";
                Console.WriteLine(str);
                Helper.LogTofile(str);
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
                try
                {
                    var myStochItem = new MyStochResult();
                    myStochItem.Oscillator = stochResults[i].K;
                    myStochItem.Signal = stochResults[i].D;
                    myStochItem.PercentJ = stochResults[i].J;
                    myStochItem.Date = stochResults[i].Date;
                    myStochItem.longSignal = false;
                    myStochItem.sma3 = sma[i].Sma;
                    myStochList.Add(myStochItem);
                    var str = $"{myStochItem.Date} k: {myStochItem.K:N4} D:{myStochItem.D:N4} Oscillator:{myStochItem.Oscillator:N4}  signal:{myStochItem.Signal:N4} 3sma:{myStochItem.sma3:N4} longSignal:{myStochItem.longSignal:N4} ";

                    Console.WriteLine(str);
                }
                catch (Exception e)
                {

                    throw;
                }

            }
            return myStochList;
        }
    }
}
