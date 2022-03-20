using Newtonsoft.Json;
using System.Net.Http.Headers;
using TradeBot.Concept;
using TradeBot.Models;
using TradeBot.Models.Enum;
using TradeBot.Repositories;
using static System.Net.WebRequestMethods;

namespace TradeBot.Services
{
    public class FinehubCrypoApi : IGetPriceApi
    {
        private readonly ApplicationDbContext _context;
        private readonly CandleRepository _candleRepository;

        public FinehubCrypoApi(ApplicationDbContext context, CandleRepository candleRepository)
        {
            _context = context;
            _candleRepository = candleRepository;
        }
        const string baseUrl = "https://finnhub.io/api/v1/crypto/";
        const string myToken = "c8hireaad3i9rgv9mbjg";
        public Root GetPrice(CryptoType coinName, ResolutionType timeFrame, DateTime from, DateTime? to)
        {
            from = from.ToUniversalTime();
            Resolution.Resolutions.TryGetValue(timeFrame, out string res);
            if (!to.HasValue)
            {
                to = DateTime.Now.ToUniversalTime();
            }
            Int32 fromTimestamp = (Int32)(from.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            Int32 toTimestamp = (Int32)(to?.Subtract(new DateTime(1970, 1, 1)))?.TotalSeconds;
            var url = baseUrl + $"candle?symbol=BINANCE:{coinName.ToString().ToUpper()}USDT&resolution={res}&from={fromTimestamp}&to={toTimestamp}&token=" + myToken;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var response = ApiRequest.Get(url);

            Root myDeserializedClass = new();
            var candles = new List<Candle>();
            if (response != null)
            {
                myDeserializedClass = JsonConvert.DeserializeObject<Root>(response);

                for (int i = 0; i < myDeserializedClass.t.Count; i++)
                {
                    var candle = new Candle(coinName, myDeserializedClass.h[i], myDeserializedClass.l[i], myDeserializedClass.o[i], myDeserializedClass.c[i], myDeserializedClass.t[i], myDeserializedClass.v[i]);
                    candles.Add(candle);
                }
                _candleRepository.InsertCandles(candles);
            }

            return myDeserializedClass;
        }

        public Root GetPrice30Min(CryptoType coinName)
        {
            return GetPrice(coinName, ResolutionType._30min, DateTime.Now.AddMinutes(-30), DateTime.Now);
        }

        public Root GetPrice5Min(CryptoType coinName, int? delay)
        {
            return GetPrice(coinName, ResolutionType._5min, DateTime.Now.AddMinutes(-30), DateTime.Now);
        }

        public Root GetPrice1Min(CryptoType coinName, int? delay)
        {
            return GetPrice(coinName, ResolutionType._1min, DateTime.Now.AddMinutes(-30), DateTime.Now);
        }
    }
}
