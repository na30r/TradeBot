using TradeBot.Models.Enum;

namespace TradeBot.Services
{
    public class timerTest2
    {
        private FinehubCrypoApi formMain;
        private readonly GhasedakSMSProvider smsProvider;
        private SkenderIndicators SkenderIndicators { get; }

        public timerTest2(FinehubCrypoApi formMain, GhasedakSMSProvider smsProvider, SkenderIndicators skenderIndicators)
        {
            this.formMain = formMain;
            this.smsProvider = smsProvider;
            SkenderIndicators = skenderIndicators;
        }

        public void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            var s = formMain.GetPrice30Min(TradeBot.Models.Enum.CryptoType.Ada);
            SkenderIndicators.StochStrategy30min(CryptoType.Ada, DateTime.Now.AddDays(-1), DateTime.Now);
        }

    }
}
