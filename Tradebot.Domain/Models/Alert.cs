﻿using TradeBot.Domain.Models.Enum;

namespace TradeBot.Domain.Models
{
    public class Alert
    {
        public int Id { get; set; }
        public CryptoType CryptoName { get; set; }
        public DateTime Date { get; set; }
        public SignalType SignalType { get; set; }
        public ResolutionType TimeFrame { get; set; }
        public Strategy Strategy { get; set; }
        public bool Result { get; set; }
    }
 
}
