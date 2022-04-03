using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TradeBot.Domain.Models;
using TradeBot.Domain.Models.Enum;
using WindowsServiceV1.Services;

namespace WindowsServiceV1
{
    public class Heartbeat
    {
        private readonly System.Timers.Timer _timer;
        bool IsSync = false;
        public Heartbeat()
        {
            _timer = new System.Timers.Timer(60000 * 5) { AutoReset = true };
          //  _timer = new System.Timers.Timer(6000) { AutoReset = false };
            _timer.Elapsed += TimerElapsed;
        }
        private async void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            string[] lies = new string[] { "**************" + DateTime.Now.ToString() + "**************" };
            var ghas = new GhasedakSMSProvider();
            File.AppendAllLines(@"C:\myService\mynote3.txt", lies);
            var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionBuilder.UseSqlServer(Helper.GetConnectionString());
            using (ApplicationDbContext ctx = new ApplicationDbContext(optionBuilder.Options))
            {
                var repo = new CandleRepository(ctx);
                //var de = repo.GetCandles(CryptoType.Ada,ResolutionType._30min, DateTime.Now.AddDays(-5), DateTime.Now);
                //if (de.FirstOrDefault() != null)
                //{
                //    Helper.LogTofile(de.FirstOrDefault().DateTime + de.FirstOrDefault().Open.ToString()); ;
                //}
                var finehub = new FinehubCrypoApi(repo);
                var skender = new SkenderIndicators(ctx, repo, ghas);
                if (IsSync == false)
                {
                    Helper.LogTofile("syncing ...");
                    finehub.SyncMarketData(new List<CryptoType> { CryptoType.btc, CryptoType.Ada, CryptoType.bnb, CryptoType.Matic, CryptoType.Sol,CryptoType.Xrp,CryptoType.Gala,CryptoType.Waves,CryptoType.Luna , CryptoType.Eth });
                    IsSync = true;
                }
                else
                {
                    Helper.LogTofile($"دریافت دیتا در ساعت " + DateTime.Now);
                    finehub.GetPrice30Min(CryptoType.Ada);
                    finehub.GetPrice30Min(CryptoType.btc);
                    finehub.GetPrice30Min(CryptoType.bnb);
                  //  finehub.GetPrice4Hour(CryptoType.bnb);
                    finehub.GetPrice30Min(CryptoType.Matic);
                    finehub.GetPrice30Min(CryptoType.Sol);
                    finehub.GetPrice30Min(CryptoType.Xrp);

                    finehub.GetPrice1Hour(CryptoType.Ada);
                    finehub.GetPrice1Hour(CryptoType.btc);
                    finehub.GetPrice1Hour(CryptoType.bnb);
                    finehub.GetPrice1Hour(CryptoType.Matic);
                    finehub.GetPrice1Hour(CryptoType.Sol);
                    finehub.GetPrice1Hour(CryptoType.Xrp);
                    finehub.GetPrice1Hour(CryptoType.Gala);
                    finehub.GetPrice1Hour(CryptoType.Luna);
                    finehub.GetPrice1Hour(CryptoType.Waves);
                    finehub.GetPrice1Hour(CryptoType.Eth);
                }
                skender.StochStrategy30min(CryptoType.btc, DateTime.Now.AddDays(-2), DateTime.Now);
                skender.StochStrategy30min(CryptoType.Ada, DateTime.Now.AddDays(-2), DateTime.Now);
                skender.StochStrategy30min(CryptoType.bnb, DateTime.Now.AddDays(-2), DateTime.Now);
                skender.StochStrategy30min(CryptoType.Sol, DateTime.Now.AddDays(-2), DateTime.Now);
                skender.StochStrategy30min(CryptoType.Matic, DateTime.Now.AddDays(-2), DateTime.Now);
                skender.StochStrategy30min(CryptoType.Xrp, DateTime.Now.AddDays(-2), DateTime.Now);


                skender.StochStrategy1Hour(CryptoType.btc, DateTime.Now.AddDays(-2), DateTime.Now);
                skender.StochStrategy1Hour(CryptoType.Ada, DateTime.Now.AddDays(-2), DateTime.Now);
                skender.StochStrategy1Hour(CryptoType.bnb, DateTime.Now.AddDays(-2), DateTime.Now);
                skender.StochStrategy1Hour(CryptoType.Sol, DateTime.Now.AddDays(-2), DateTime.Now);
                skender.StochStrategy1Hour(CryptoType.Matic, DateTime.Now.AddDays(-2), DateTime.Now);
                skender.StochStrategy1Hour(CryptoType.Xrp, DateTime.Now.AddDays(-2), DateTime.Now);
                skender.StochStrategy1Hour(CryptoType.Luna, DateTime.Now.AddDays(-2), DateTime.Now);
                skender.StochStrategy1Hour(CryptoType.Waves, DateTime.Now.AddDays(-2), DateTime.Now);
                skender.StochStrategy1Hour(CryptoType.Gala, DateTime.Now.AddDays(-2), DateTime.Now);
                skender.StochStrategy1Hour(CryptoType.Eth, DateTime.Now.AddDays(-2), DateTime.Now);
                Console.WriteLine(DateTime.Now);

            }
        }
        public void start()
        {
            _timer.Start();
        }
        public void Stop()
        {
            IsSync = false;
            var ghas = new GhasedakSMSProvider();
            ghas.AlertString("وایساد");
            Helper.LogTofile("وایساد");
            _timer.Stop();
        }
    }
}
