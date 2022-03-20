using Skender.Stock.Indicators;
using System.ComponentModel.DataAnnotations;
using TradeBot.Models.Enum;

namespace TradeBot.Models
{
    public class Candle
    {

        [Key]
        public int ID { get; set; }
        public int CoinName { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public int Time { get; set; }
        public double v { get; set; }
        public DateTime DateTime { get; set; }

        public Candle()
        {

        }
        public Candle(CryptoType coinName, double high, double low, double open, double close, int time, double v)
        {
            CoinName =(int) coinName;
            High = high;
            Low = low;
            Open = open;
            Close = close;
            Time = time;
            DateTime = time.UnixTimeStampToDateTime();
            this.v = v;
        }

        public static implicit operator Quote(Candle candle)
        {
            return new Quote() {
            Close = (decimal) candle.Close,
            Low = (decimal)candle.Low,
            Open = (decimal)candle.Open,
            High = (decimal)candle.High,
            Date = candle.DateTime,
            Volume =(decimal) candle.v
            };
        }
    }
}
