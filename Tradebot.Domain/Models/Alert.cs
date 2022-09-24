using TradeBot.Domain.Models.Enum;

namespace TradeBot.Domain.Models
{
    public class Alert
    {
        public int Id { get; set; }
        public CryptoType CryptoName { get; set; }
        public DateTime Date { get; set; }
        public SignalType SignalType { get; set; }

        protected int timeframe;

        public ResolutionType Timeframe
        {
            get { return (ResolutionType)timeframe; }
            set { timeframe = (int)value; }
        }

        public Strategy Strategy
        {
            get { return (Strategy)strategy; }
            set { strategy = (int)value; }
        }

        protected int strategy;
        public bool Result { get; set; }
    }
 
}
