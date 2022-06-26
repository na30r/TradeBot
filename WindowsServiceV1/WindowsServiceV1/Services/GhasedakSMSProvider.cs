using System.Text;
using System.Threading;
using System.Timers;
using TradeBot.Domain.Models;
using TradeBot.Domain.Models.Enum;
using WindowsServiceV1;
using WindowsServiceV1.Concept;

namespace WindowsServiceV1.Services
{
    public static class GhasedakSMSProvider  //: INotificationService
    {
        private static readonly string _ghasedakApiKey = "c0a85deb17de6cb92ca2d012b3ea07209e557240e7f8500300c5af977a7f6a70";
        private static readonly string _myPhoneNumber = "09370963830";
        private static readonly string _ghasedakLineNumber = "10008566";
        private static System.Timers.Timer _timer;
        private static List<Alert> AlertList = new();
        private static int timerIntervalInSecond = 10;
        public static void Alert(string content)
        {
            throw new NotImplementedException();
        }
        public static void Alert(Alert alert)
        {
            var sms = new Ghasedak.Core.Api(_ghasedakApiKey);
            ContentBuilder contentBuilder = new();
            var res = contentBuilder.MessageBuilder(alert);
            var x = sms.SendSMSAsync(res.ToString(), _myPhoneNumber, _ghasedakLineNumber).Result;

            var s = x;
        }
        public static void AlertString(string alert)
        {
            try
            {
                var sms = new Ghasedak.Core.Api(_ghasedakApiKey);
                ContentBuilder contentBuilder = new();
                var x = sms.SendSMSAsync(alert, _myPhoneNumber, _ghasedakLineNumber).Result;
            }
            catch (Exception e)
            {

            }
        }

        public static void BuyAlert(Strategy strategy, ResolutionType resolution, CryptoType cryptoType)
        {
            AlertList.Add(new Alert()
            {
                CryptoName = cryptoType,
                Date = DateTime.Now,
                Strategy = strategy,
                TimeFrame = resolution,
                SignalType = SignalType.buy
            });

            if (_timer == null || !_timer.Enabled)
            {
                _timer = new System.Timers.Timer(timerIntervalInSecond * 1000) { AutoReset = false };
                _timer.Elapsed += TimerElapsede;
                _timer.Start();

            }
            else
            {
                _timer.Stop();
                _timer.Start();
            }

        }
        public static void TimerElapsede(object sender, ElapsedEventArgs e)
        {
            var groupedAlert = AlertList.GroupBy(x => x.Strategy).ToList();
            AlertList.Clear();
            foreach (var item in groupedAlert)
            {
                SendMessageBasedOnStrategy(item);
            }
        }

        private static void SendMessageBasedOnStrategy(IGrouping<Strategy, Alert> item)
        {
            var cryptos = string.Join( "و",item.Select(a => a.CryptoName));
            var mystring = $" هشدار خرید برای ارز {cryptos} ";
            var timeframe = item.Select(a => a.TimeFrame).Distinct();
            if (timeframe.Count() == 1)
            {
                mystring += $"در تایم فریم {timeframe.FirstOrDefault()}";
            }
            Helper.LogTofile(mystring);
            AlertString(mystring);
        }
    }

}