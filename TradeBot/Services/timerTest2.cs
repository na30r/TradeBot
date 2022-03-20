namespace TradeBot.Services
{
    public class timerTest2
    {
        private FinehubCrypoApi formMain;
        private readonly GhasedakSMSProvider smsProvider;

        public timerTest2(FinehubCrypoApi formMain, GhasedakSMSProvider smsProvider)
        {
            this.formMain = formMain;
            this.smsProvider = smsProvider;
        }

        public void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {

            var s = formMain.GetPrice30Min(TradeBot.Models.Enum.CryptoType.btc);
        }

    }
}
