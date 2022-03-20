//using Newtonsoft.Json;
//using System.Net.Http.Headers;
//using TradeBot.Concept;
//using TradeBot.Models;

//namespace TradeBot.Services
//{
//    public class NomicsCryptoApi : IGetPriceApi
//    {
//        public string GetPrice(string coinName)
//        {
//            HttpClient client = new HttpClient();
//            client.BaseAddress = new Uri("https://api.nomics.com");
//            client.DefaultRequestHeaders.Accept.Clear();
//            client.DefaultRequestHeaders.Accept.Add(
//                new MediaTypeWithQualityHeaderValue("application/json"));


//            var strings = "https://api.nomics.com/v1/currencies/ticker?key=37a3d8d587e4d30348c08f9d2e635a47362780de&ids=BTC,ETH,XRP&interval=1d,30d&convert=EUR&platform-currency=ETH&per-page=100&page=1";


//            HttpResponseMessage response = client.GetAsync(strings).Result;
//            List<Root> myDeserializedClass = new();
//            if (response.IsSuccessStatusCode)
//            {
//                var res = response.Content.ReadAsStringAsync().Result;
//                myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(res);
//            }
//            return myDeserializedClass.ToString();
//        }

//        public List<string> GetPrice(string[] coinNames)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
