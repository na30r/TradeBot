using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tradebot.Domain.ViewModels.Candle
{
    public class CandleDto
    {
        public DateTime Datetime { get; set; }
        public string CoinName { get; set; }
    }
}
