using Skender.Stock.Indicators;
using TradeBot.Domain.Models;
using TradeBot.Domain.Models.Enum;
using WindowsServiceV1.Models;

namespace WindowsServiceV1.Strategies
{
    public class MAStrategy : BaseStrategy
    {
        private IEnumerable<SmmaResult> smma21;
        private IEnumerable<SmmaResult> smma50;
        private IEnumerable<SmmaResult> smma200;
        private List<CandlePosition> candles;
        private SignalType? openPosition;
        public const float sma200MinimumPercentage = 1; //kamtarin darsad baraye vorood be position
        public const float sma21MinimumPercentage = 0.5F;//kamtarin darsad baraye vorood be position
        public const int closePositionAfterCandles = 15;//close position after how many candles

        public override List<CandlePosition> Strategy1Hour(List<CandlePosition> candles, CryptoType cryptoType)
        {

            return new List<CandlePosition>();
        }
        private void SetSmma(List<CandlePosition> candles)
        {
            var quotes = candles.Select(a => (Quote)a).ToList();
            this.candles = candles;
            smma21 = quotes.GetSmma(21);
            smma50 = quotes.GetSmma(50);
            smma200 = quotes.GetSmma(200);
        }

        /// <summary>
        /// 1-Avoid starting long positon 2-set Tp for open Positions 
        /// </summary>
        /// <param name="candlePositions"></param>
        /// <returns></returns>
        public List<CandlePosition> TPStrategy(List<CandlePosition> candlePositions)
        {
            SetSmma(candlePositions);
            AvoidStartFalsePositions();
            SetTPforOpenPositions();
            return candles;
        }

        private void SetTPforOpenPositions()
        {
            var OpenPositionCandleNumber = 0;
            var zippedSma = smma21.Zip(smma50, smma200);// first => smma21  second => smma50   third => smma200
            var zippedObj = candles.Zip(zippedSma);
            foreach (var obj in zippedObj)
            {
                if (obj.First.SignalType == SignalType.buy || openPosition == SignalType.buy)
                {
                    openPosition = SignalType.buy;
                    OpenPositionCandleNumber++;
                    var closeTemp = obj.First.Close;
                    var sma21Temp = (double?)obj.Second.First.Smma;
                    var sma50Temp = (double?)obj.Second.Second.Smma;
                    var sma200Temp = (double?)obj.Second.Third.Smma;
                    Console.WriteLine("close : " + obj.First.Close + " smma21 " + obj.Second.First.Smma);
                    if (closeTemp < sma21Temp)
                    {
                        obj.First.SetTP(sma21Temp, Strategy.Stoch);
                        Console.WriteLine($"set tp {sma21Temp} for {obj.First.DateTime}");
                    }
                    else if (closeTemp > sma21Temp && closeTemp < sma200Temp)
                    {
                        obj.First.SetTP(sma200Temp, Strategy.Stoch);
                        Console.WriteLine($"set tp {sma200Temp} for {obj.First.DateTime}");
                    }
                    if (obj.First.IsTPHitted() || OpenPositionCandleNumber == closePositionAfterCandles)
                    {
                        //obj.First.ClosePosition();
                        openPosition = null;
                        OpenPositionCandleNumber = 0;
                    }
                }
            }
            candles = zippedObj.Select(a => a.First).ToList();
        }

        private void AvoidStartFalsePositions()
        {

            var zippedSma = smma21.Zip(smma50, smma200);// first => smma21  second => smma50   third => smma200
            //first  => candles 
            //second => smma
            var zippedObj = candles.Zip(zippedSma);
            foreach (var obj in zippedObj)
            {
                if (obj.First.SignalType == SignalType.buy)
                {
                    var closeTemp = obj.First.Close;
                    var sma21Temp = (double?)obj.Second.First.Smma;
                    var sma50Temp = (double?)obj.Second.Second.Smma;
                    var sma200Temp = (double?)obj.Second.Third.Smma;
                    Console.WriteLine("time of buy signal :" + obj.First.DateTime);
                    Console.WriteLine("close : " + obj.First.Close + " smma21 " + obj.Second.First.Smma);
                    if (closeTemp > sma21Temp && closeTemp > sma200Temp)
                    {
                        // dar in haalat karish nadarim
                    }
                    else if ((sma21Temp / closeTemp < sma21MinimumPercentage / 100 && sma21Temp / closeTemp > 1) || (sma200Temp / closeTemp < sma200MinimumPercentage / 100 && sma200Temp / closeTemp > 1))
                    {
                        obj.First.AvoidPosotion();
                        Console.WriteLine($"avoid position on {obj.First.DateTime}");
                    }
                    Console.WriteLine((double?)obj.Second.First.Smma / obj.First.Close);
                    Console.WriteLine(obj.First.Close / (double?)obj.Second.First.Smma);
                    if ((double?)obj.Second.First.Smma / obj.First.Close > 0.01)
                    {
                        Console.WriteLine($"avoid position on {obj.First.DateTime}");
                        Console.WriteLine((double?)obj.Second.Third.Smma / obj.First.Close);
                    }

                }
            }
            candles = zippedObj.Select(a => a.First).ToList();
        }
    }
}
