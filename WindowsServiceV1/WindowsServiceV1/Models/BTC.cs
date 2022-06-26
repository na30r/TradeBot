using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeBot.Domain.Models.Enum;
using WindowsServiceV1.Services;

namespace WindowsServiceV1.Models
{
    public class CryptoGetPrice 
    {
        private FinehubCrypoApi _finehub;
        private  CryptoType CryptoType { get; set; }
        public CryptoGetPrice(CryptoType cryptoType ,FinehubCrypoApi finehubCrypoApi)
        {
            _finehub=finehubCrypoApi;
            CryptoType = cryptoType;
        }
        private string cryptoName = "btc";
        public event EventHandler<PriceChangeEventArgs> onpriceChangeCrypto30min;
        public event EventHandler<PriceChangeEventArgs> onpriceChangeCrypto1Hour;

        public void PriceChange30min()
        {
            var res =_finehub.GetPrice30Min(CryptoType);
            if (res)
            {
                var args = new PriceChangeEventArgs(CryptoType, ResolutionType._30min);
                onpriceChangeCrypto30min.Invoke(this, args);
            }
        }
        public void PriceChange1Hour()
        {
            var res = _finehub.GetPrice1Hour(CryptoType);
            if (res)
            {
                var args = new PriceChangeEventArgs(CryptoType, ResolutionType._30min);
                onpriceChangeCrypto1Hour.Invoke(this, args);
            }
        }

    }

    public class PriceChangeEventArgs : EventArgs
    {
        public CryptoType CryptoType { get; set; }
        public ResolutionType TimeFrame { get; set; }
        public PriceChangeEventArgs(CryptoType cryptoType, ResolutionType timeFrame)
        {
            CryptoType = cryptoType;
            TimeFrame = timeFrame;
        }
    }

}
