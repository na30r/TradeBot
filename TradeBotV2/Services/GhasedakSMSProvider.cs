using System.Text;
using TradeBot.Concept;
using TradeBot.Models;

namespace TradeBot.Services
{
    public class GhasedakSMSProvider : INotificationService
    {

        private readonly string _ghasedakApiKey = "c0a85deb17de6cb92ca2d012b3ea07209e557240e7f8500300c5af977a7f6a70";
        private readonly string _myPhoneNumber = "09370963830";
        private readonly string _ghasedakLineNumber = "10008566";

        public void Alert(string content)
        {
            throw new NotImplementedException();
        }
        public void Alert(Alert alert)
        {
            var sms = new Ghasedak.Core.Api(_ghasedakApiKey);
            ContentBuilder contentBuilder = new();
            var res = contentBuilder.MessageBuilder(alert);
            var x = sms.SendSMSAsync(res.ToString(), _myPhoneNumber, _ghasedakLineNumber).Result;

            var s = x;
        }
        public void AlertString(string alert)
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

    }
}