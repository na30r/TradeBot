
using Newtonsoft.Json;
using System.Net.Http.Headers;
using TradeBot.Domain.Models;
using TradeBot.Domain.Models.Enum;
using WindowsServiceV1;
using WindowsServiceV1.Concept;
using static System.Net.WebRequestMethods;

namespace WindowsServiceV1.Services
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

        public string CreateBaseUrl(CryptoType coinName, ResolutionType timeFrame, int fromTimestamp, int toTimestamp)
        {
            Resolution.Resolutions.TryGetValue(timeFrame, out string res);
            return baseUrl + $"candle?symbol=BINANCE:{coinName.ToString().ToUpper()}USDT&resolution={res}&from={fromTimestamp}&to={toTimestamp}&token=" + myToken;
        }

        public bool GetPrice(CryptoType coinName, ResolutionType timeFrame, DateTime from, DateTime? to)
        {
            FinehubCandle myDeserializedClass = new();
            var notApprovedList = _candleRepository.GetNotApprovedList(coinName, timeFrame);
            if (notApprovedList.Count > 1)
            {
                from = notApprovedList.LastOrDefault().DateTime;
            }
            if (!_candleRepository.GetLastCandle(coinName, timeFrame).IsExpired())
            {
                return true;
            }
            var candles = new List<Candle>();
            try
            {
                from = from.ToUniversalTime();
                if (!to.HasValue)
                {
                    to = DateTime.Now.ToUniversalTime();
                }
                int fromTimestamp = (int)from.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                int toTimestamp = (int)(to?.Subtract(new DateTime(1970, 1, 1)))?.TotalSeconds;
                var url = CreateBaseUrl(coinName, timeFrame, fromTimestamp, toTimestamp);

                var response = ApiRequest.Get(url);


                if (response != null)
                {
                    myDeserializedClass = JsonConvert.DeserializeObject<FinehubCandle>(response);
                    Helper.LogTofile($"Deserilized {coinName}");
                    for (int i = 0; i < myDeserializedClass.t.Count; i++)
                    {
                        var candle = new Candle(coinName, myDeserializedClass.h[i], myDeserializedClass.l[i], myDeserializedClass.o[i], myDeserializedClass.c[i], myDeserializedClass.t[i], myDeserializedClass.v[i], timeFrame);
                        if (DateTime.Now.Subtract(candle.DateTime).TotalMinutes > Resolution.ResolutionsInMinute[timeFrame])
                        {
                            candle.Approved = true;
                        }
                        candles.Add(candle);
                    }
                    _candleRepository.InsertCandles(candles, timeFrame);
                }
                return true;
            }
            catch (Exception e)
            {
                System.IO.File.AppendAllLines(@"C:\myService\mynote3.txt", new List<string> { $" {e.Message} + from finehub GetPrice method for {coinName}" });
                Console.WriteLine();
                return false;
            }
        }
        public bool GetPrice4Hour(CryptoType coinName)
        {
            return GetPrice(coinName, ResolutionType._4h, DateTime.Now.AddDays(-1), DateTime.Now);
        }
        public bool GetPrice1Hour(CryptoType coinName)
        {
            return GetPrice(coinName, ResolutionType._1h, DateTime.Now.AddHours(-8), DateTime.Now);
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
                    Helper.LogTofile("2");
                    var lastCandle30min = _candleRepository.GetLastApprovedCandle(crypto, ResolutionType._30min);
                    var lastCandle1H = _candleRepository.GetLastApprovedCandle(crypto, ResolutionType._1h);
                    if (lastCandle30min == null)
                    {
                        Helper.LogTofile("get candles from 3 days ago");
                        GetPrice(crypto, ResolutionType._30min, DateTime.Now.AddDays(-3), DateTime.Now);
                    }
                    else if (!_candleRepository.IsSync(lastCandle30min))
                    {
                        Helper.LogTofile("get candles from last candle");
                        GetPrice(crypto, ResolutionType._30min, lastCandle30min.DateTime, DateTime.Now);
                    }

                    if (lastCandle1H == null)
                    {
                        Helper.LogTofile("get candles from 3 days ago");
                        GetPrice(crypto, ResolutionType._1h, DateTime.Now.AddDays(-3), DateTime.Now);
                    }
                    else if (!_candleRepository.IsSync(lastCandle1H))
                    {
                        Helper.LogTofile("get candles from last candle");
                        GetPrice(crypto, ResolutionType._1h, lastCandle1H.DateTime, DateTime.Now);
                    }
                    Console.WriteLine($"{crypto } has been logged ");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "from finehub SyncMarketData method");
            }

        }
    }
}
