using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeBot.Domain.Models.Enum;

namespace Tradebot.Domain.ViewModels.Candle
{
    public class CandleDto
    {
        public DateTime Datetime { get; set; }
        public string CoinName { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public int Time { get; set; }
        public string SignalType { get; set; }
        public double? BuyPrice { get; set; }
        public double? SellPrice { get; set; }
        public double? TPPrice { get; set; }
        public double? SLPrice { get; set; }

        public Strategy SignalStrategy { get; set; }
        public Strategy TPStrategy { get; set; }
        public Strategy SLStrategy { get; set; }

        public float ROE { get; set; }
    }
}
