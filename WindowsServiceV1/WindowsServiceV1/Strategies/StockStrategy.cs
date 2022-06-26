using Skender.Stock.Indicators;
using TradeBot.Domain.Models;
using TradeBot.Domain.Models.Enum;
using WindowsServiceV1.Services;

namespace WindowsServiceV1.Strategies
{
    public class StockStrategy : BaseStrategy
    {
        //public StockStrategy( CandleRepository candleRepository) : base(candleRepository)
        //{
        //}
        public void Strategy4h(List<CandlePosition> candles, CryptoType cryptoType)
        {
            var res = StochStrategy(candles,cryptoType, ResolutionType._4h);
            LogStrategy(res, cryptoType, ResolutionType._4h);
        }
        public List<CandlePosition> Strategy30min(List<CandlePosition> candles, CryptoType cryptoType)
        {
            var res = StochStrategy(candles,cryptoType, ResolutionType._30min);
            LogStrategy(res, cryptoType, ResolutionType._30min);
            return res;
        }
        public override List<CandlePosition> Strategy1Hour(List<CandlePosition> candles, CryptoType cryptoType)
        {
            var res = StochStrategy(candles,cryptoType, ResolutionType._1h);
            LogStrategy(res, cryptoType, ResolutionType._1h);
            return res;
        }
        public List<CandlePosition> Strategy1Hour(List<Candle> candles, CryptoType cryptoType) => Strategy1Hour(candles.Select(a =>  new CandlePosition(a)).ToList(), cryptoType);
        public List<CandlePosition> Strategy30min(List<Candle> candles, CryptoType cryptoType) =>
            Strategy30min(candles.Select(a => new CandlePosition(a)).ToList(), cryptoType);

        public void StrategyDaily(List<CandlePosition> candles,CryptoType cryptoType)
        {
            var res = StochStrategy(candles,cryptoType, ResolutionType.Daily);
            LogStrategy(res, cryptoType, ResolutionType.Daily);
        }

        private List<CandlePosition> StochStrategy(List<CandlePosition> candles, CryptoType cryptoType, ResolutionType resolution)
        {
            Helper.LogTofile($"enter stock cryptotype: {cryptoType} resolution: {resolution} ");
       //   IEnumerable<Quote> quotes = _candleRepository.GetCandles(cryptoType, resolution, from, to).Select(a => (Quote)a).ToList();
            IEnumerable<Quote> quotes = candles.Select(a => (Quote)a).ToList();

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
                    candles[i].SignalType = SignalType.buy;
                    candles[i].SignalStrategy = Strategy.Stoch;
                    candles[i].BuyPrice = candles[i].Close;
                     var str = $"=>{cryptoType} in timeframe {resolution}  SMA on {myStochList[i].Date}  k: {myStochList[i].K:N4} D:{myStochList[i].D:N4} 3sma:{myStochList[i].sma3:N4} longSignal:{myStochList[i].longSignal:N4} ";
                }
            }

            return candles;

            //k abie d ghermeze 

            //k>15
            //k> miangin
        }

        public void LogStrategy(List<CandlePosition> myStochList, CryptoType cryptoType, ResolutionType resolution)
        {
            var lastCandle = myStochList.TakeLast(2).FirstOrDefault();
            if (lastCandle != null && lastCandle.SignalType == SignalType.buy)
            {
                GhasedakSMSProvider.BuyAlert(Strategy.Stoch, resolution, cryptoType);
            }
            var longSignals = myStochList.Where(a => a.SignalType == SignalType.buy).ToList();
            foreach (CandlePosition r in longSignals)
            {
               // var str = $"=>{cryptoType} in timeframe {resolution}  SMA on {r.Date}  k: {r.K:N4} D:{r.D:N4} 3sma:{r.sma3:N4} longSignal:{r.longSignal:N4} ";
                var str = $"=>{cryptoType} in timeframe {resolution}  SMA on {r.DateTime} longSignal:{r.SignalType} ";
                Console.WriteLine(str);
                Helper.LogTofile(str);
            }
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
