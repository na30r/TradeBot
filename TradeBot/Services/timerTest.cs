using TradeBot.Models;
using TradeBot.Repositories;

namespace TradeBot.Services
{
    public class timerTest
    {

        private readonly FinehubCrypoApi _fineHub;
        private readonly GhasedakSMSProvider _smsProvider;

        public timerTest(FinehubCrypoApi fineHub, GhasedakSMSProvider smsProvider)
        {
            _fineHub = fineHub;
            _smsProvider = smsProvider;
        }
        private FinehubCrypoApi formMain;
        private readonly GhasedakSMSProvider smsProvider;

        public  void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            var s = formMain.GetPrice30Min(TradeBot.Models.Enum.CryptoType.btc);
        }

        public void SendStartMessage(Object source, System.Timers.ElapsedEventArgs e)
        {
            Example.Fire(2, OnTimedEvent);
            smsProvider.AlertString("اپ وزین ترید استارت خورد .");
        }
    }
}
