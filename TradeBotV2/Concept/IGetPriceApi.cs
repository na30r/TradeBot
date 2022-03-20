using TradeBot.Models;
using TradeBot.Models.Enum;

namespace TradeBot.Concept
{
    public interface IGetPriceApi
    {
        Root GetPrice(CryptoType coinName,ResolutionType timeFrame, DateTime from, DateTime? to);

        Root GetPrice30Min(CryptoType coinName);
        Root GetPrice5Min(CryptoType coinName,int? delay);
        Root GetPrice1Min(CryptoType coinName,int? delay);
    }
}
