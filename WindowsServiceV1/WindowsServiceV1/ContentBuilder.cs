using System.Text;
using TradeBot.Domain.Models;
using WindowsServiceV1;

namespace WindowsServiceV1
{
    public class ContentBuilder
    {

        public StringBuilder MessageBuilder(Alert alert)
        {
            StringBuilder stringBuilder = new();
            stringBuilder.Append($" هشدار خرید {alert.CryptoName.ToString()} در تایم فریم {alert.TimeFrame.GetDescription()} ");
            return stringBuilder;
        }
    }
}
