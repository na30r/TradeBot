using Newtonsoft.Json;
using NuGet.Packaging.Signing;
using System.Net.Http.Headers;
using TradeBot.Concept;
using TradeBot.Models;
using TradeBot.Models.Enum;
using TradeBot.Repositories;
using TradeBotV2.Repositories;
using static System.Net.WebRequestMethods;

namespace TradeBot.Services
{
    public class FinehubCrypoApi : IGetPriceApi<FinehubCandle>
    {
        private readonly CandleRepository _candleRepository;
        const string baseUrl = "https://finnhub.io/api/v1/crypto/";
        const string myToken = "c8hireaad3i9rgv9mbjg";
        public FinehubCrypoApi(CandleRepository candleRepository)
        {
            _candleRepository = candleRepository;
        }

        public string CreateBaseUrl(CryptoType coinName ,ResolutionType timeFrame, Int32 fromTimestamp, Int32 toTimestamp)
        {
            Resolution.Resolutions.TryGetValue(timeFrame, out string res);
            return baseUrl + $"candle?symbol=BINANCE:{coinName.ToString().ToUpper()}USDT&resolution={res}&from={fromTimestamp}&to={toTimestamp}&token=" + myToken;
        }

        public bool GetPrice(CryptoType coinName, ResolutionType timeFrame, DateTime from, DateTime? to)
        {
            FinehubCandle myDeserializedClass = new();
            var candles = new List<Candle>();
            try
            {
                from = from.ToUniversalTime();
                if (!to.HasValue)
                {
                    to = DateTime.Now.ToUniversalTime();
                }
                Int32 fromTimestamp = (Int32)(from.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                Int32 toTimestamp = (Int32)(to?.Subtract(new DateTime(1970, 1, 1)))?.TotalSeconds;
                var url = CreateBaseUrl(coinName, timeFrame, fromTimestamp, toTimestamp);

                var response = ApiRequest.Get(url);

              
                if (response != null)
                {
                    myDeserializedClass = JsonConvert.DeserializeObject<FinehubCandle>(response);

                    for (int i = 0; i < myDeserializedClass.t.Count; i++)
                    {
                        var candle = new Candle(coinName, myDeserializedClass.h[i], myDeserializedClass.l[i], myDeserializedClass.o[i], myDeserializedClass.c[i], myDeserializedClass.t[i], myDeserializedClass.v[i], timeFrame);
                        candles.Add(candle);
                    }
                    _candleRepository.InsertCandles(candles);
                }
                return true;
            }
            catch (Exception e )
            {
                Console.WriteLine($" {e.Message} + from finehub GetPrice method for {coinName}");
                return false;
            }
        }

        public bool GetPrice30Min(CryptoType coinName)
        {
            return GetPrice(coinName, ResolutionType._30min, DateTime.Now.AddMinutes(-90), DateTime.Now);
        }

        public bool GetPrice5Min(CryptoType coinName, int? delay)
        {
            return GetPrice(coinName, ResolutionType._5min, DateTime.Now.AddMinutes(-30), DateTime.Now);
        }

        public bool GetPrice1Min(CryptoType coinName, int? delay)
        {
            return GetPrice(coinName, ResolutionType._1min, DateTime.Now.AddMinutes(-30), DateTime.Now);
        }

        public void SyncMarketData(List<CryptoType> cryptos)
        {
            try
            {
                foreach (var crypto in cryptos)
                {
                    var lastCandle = _candleRepository.GetLastCandle(crypto);
                    if (!_candleRepository.IsSync(lastCandle))
                    {
                        GetPrice(crypto, ResolutionType._30min, lastCandle.DateTime, DateTime.Now);
                        Console.WriteLine($"{crypto } has been logged ");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "from finehub SyncMarketData method");
            }

        }
    }
}
