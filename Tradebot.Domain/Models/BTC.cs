//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TradeBot.Domain.Models.Enum;

//namespace Tradebot.Domain.Models
//{
//    public class CryptoBase
//    {

//    }
//    public class BTC : CryptoBase
//    {
//        public BTC(FinehubCrypoApi finehubCrypoApi)
//        {

//        }
//        private string cryptoName = "btc";
//        public event EventHandler<PriceChangeEventArgs> onpriceChangeCrypto;

//        public void PriceChange()
//        {
//            var args = new PriceChangeEventArgs();
//            onpriceChangeCrypto.Invoke(this, args);
//        }

//    }

//    public class PriceChangeEventArgs : EventArgs
//    {

//    }

//    public class Subscriber
//    {
//        public void SubscriberChange(object sender , PriceChangeEventArgs args)
//        {
//            Console.WriteLine($"sender : {sender} and args : {args}");
//        }
//    }

//}
