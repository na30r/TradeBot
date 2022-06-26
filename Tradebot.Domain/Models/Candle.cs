using Skender.Stock.Indicators;
using System.ComponentModel.DataAnnotations;
using Tradebot.Domain;
using TradeBot.Domain.Models.Enum;

namespace TradeBot.Domain.Models
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

        protected int timeframe;

        public ResolutionType Timeframe
        {
            get { return (ResolutionType)timeframe; }
            set { timeframe = (int)value; }
        }


        public DateTime DateTime { get; set; }

        public bool Approved { get; set; }

        public Candle()
        {

        }
        public Candle(CryptoType coinName, double high, double low, double open, double close, int time, double v, ResolutionType timeFrame)
        {
            CoinName = (int)coinName;
            High = high;
            Low = low;
            Open = open;
            Close = close;
            Time = time;
            DateTime = time.UnixTimeStampToDateTime();
            Approved = false;
            this.Timeframe = timeFrame;
            this.v = v;
        }

        public bool IsExpired() =>
            DateTime.Now.Subtract(DateTime).TotalMinutes > Resolution.ResolutionsInMinute[Timeframe] ? true : false;


        public static implicit operator Quote(Candle candle)
        {
            return new Quote()
            {
                Close = (decimal)candle.Close,
                Low = (decimal)candle.Low,
                Open = (decimal)candle.Open,
                High = (decimal)candle.High,
                Date = candle.DateTime,
                Volume = (decimal)candle.v
            };
        }


    }

    public class CandlePosition : Candle
    {
        public SignalType? SignalType { get; set; }

        public double? BuyPrice { get; set; }
        public double? SellPrice { get; set; }
        public double? TPPrice { get; set; }
        public double? SLPrice { get; set; }

        public Strategy SignalStrategy { get; set; }
        public Strategy TPStrategy { get; set; }
        public Strategy SLStrategy { get; set; }

        public float ROE { get; set; }

        public CandlePosition(Candle candle)
        {
            this.ID = candle.ID;
            this.CoinName = candle.CoinName;
            this.DateTime = candle.DateTime;
            this.High = candle.High;
            this.Low = candle.Low;
            this.Open = candle.Open;
            this.Close = candle.Close;
            this.Time = candle.Time;
            this.v = candle.v;
            this.Timeframe = candle.Timeframe;
            this.Approved = candle.Approved;
        }

        public bool IsTPHitted()
        {
            if (TPPrice == null)
            {
                return false;
            }
            if (TPPrice < Close || TPPrice < High)
                return true;
            return false;
        }

        public void SetTP(double? value , Strategy tPStrategy )
        {
            if (value != null && value != 0)
            {
                TPPrice = value;
                TPStrategy = tPStrategy; 
            }
        }

        public void ClosePosition()
        {
            throw new NotImplementedException();
        }

        public CandlePosition(CryptoType coinName, double high, double low, double open, double close, int time, double v, ResolutionType timeFrame)
        {
            CoinName = (int)coinName;
            High = high;
            Low = low;
            Open = open;
            Close = close;
            Time = time;
            DateTime = time.UnixTimeStampToDateTime();
            Approved = false;
            this.Timeframe = timeFrame;
            this.v = v;
        }
        public void AvoidPosotion() {
            SignalType = null;
            SignalStrategy = 0;
            BuyPrice = null;
        }

    }
}
