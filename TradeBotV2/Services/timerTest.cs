using TradeBot.Models;
using TradeBot.Models.Enum;
using TradeBot.Repositories;

namespace TradeBot.Services
{
    public class timerTest
    {

        private readonly FinehubCrypoApi _fineHub;
        private readonly GhasedakSMSProvider _smsProvider;
        private  SkenderIndicators _skenderIndicators;

        public timerTest(FinehubCrypoApi fineHub, GhasedakSMSProvider smsProvider,SkenderIndicators skenderIndicators)
        {
            _fineHub = fineHub;
            _smsProvider = smsProvider;
            _skenderIndicators = skenderIndicators;
        }

        public  void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            var s = _fineHub.GetPrice30Min(TradeBot.Models.Enum.CryptoType.Ada);
            var sd = _fineHub.GetPrice30Min(TradeBot.Models.Enum.CryptoType.btc);
            _skenderIndicators.test(CryptoType.btc, ResolutionType._30min, DateTime.Now.AddDays(-2), DateTime.Now);
            _skenderIndicators.test(CryptoType.Ada, ResolutionType._30min, DateTime.Now.AddDays(-2), DateTime.Now);
        }

        public void SendStartMessage(Object source, System.Timers.ElapsedEventArgs e)
        {
            _smsProvider.AlertString("اپ وزین ترید استارت خورد .");
            Example.Fire(OnTimedEvent);
        }
    }
}
