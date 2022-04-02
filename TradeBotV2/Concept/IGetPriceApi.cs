using TradeBot.Models;
using TradeBot.Models.Enum;

namespace TradeBot.Concept
{
    public interface IGetPriceApi<T>
    {
        bool GetPrice(CryptoType coinName,ResolutionType timeFrame, DateTime from, DateTime? to);

        bool GetPrice30Min(CryptoType coinName);
        bool GetPrice5Min(CryptoType coinName,int? delay);
        bool GetPrice1Min(CryptoType coinName,int? delay);
    }
}
