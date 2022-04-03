//using Microsoft.VisualBasic;
//using System.Net.Http.Headers;

//namespace TradeBot.Models
//{
//    public static class ApiRequest
//    {
//        public static string Get(string getUrl)
//        {
//            HttpClient client = new HttpClient();
//            //client.DefaultRequestHeaders.Accept.Clear();
//            client.DefaultRequestHeaders.Accept.Add(
//                new MediaTypeWithQualityHeaderValue("application/json"));
//            HttpResponseMessage response = client.GetAsync(getUrl).Result;
//            if (response.IsSuccessStatusCode)
//            {
//                var res = response.Content.ReadAsStringAsync().Result;
//                return res;
//            }
//            return null;
//        }
//    }
//}
