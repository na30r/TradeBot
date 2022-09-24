using Microsoft.EntityFrameworkCore;
using System.Timers;
using Tradebot.Application.Services;
using Tradebot.Data;
using Tradebot.Setting;
using TradeBot.Application.Repository;
using TradeBot.Domain.Models;
using TradeBot.Domain.Models.Enum;
using WindowsServiceV1.Services;
using WindowsServiceV1.Strategies;

namespace WindowsServiceV1
{
  public class Heartbeat
  {
    private readonly System.Timers.Timer _timer;
    bool IsSync = false;
    public Heartbeat()
    {
     // _timer = new System.Timers.Timer(45000) { AutoReset = true };
      _timer = new System.Timers.Timer(300000) { AutoReset = true }; // 5 min == 300000
      _timer.Elapsed += TimerElapsed;
    }
    private async void TimerElapsed(object sender, ElapsedEventArgs e)
    {
      // var s = SettingsConfig.GetConnectionString();
      string[] lies = new string[] { "**************" + DateTime.Now.ToString() + "**************" };
      try
      {
        File.AppendAllLines(@"C:\myService\mynote3.txt", lies);
      }
      catch (Exception)
      {

      }
      var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
      optionBuilder.UseSqlServer(Helper.GetConnectionString());
      using (ApplicationDbContext ctx = new ApplicationDbContext(optionBuilder.Options))
      {
        var candleRepo = new CandleRepository(ctx);
        var alertService = new AlertService(ctx);


        //var de = repo.GetCandles(CryptoType.Ada,ResolutionType._30min, DateTime.Now.AddDays(-5), DateTime.Now);
        //if (de.FirstOrDefault() != null)
        //{
        //    Helper.LogTofile(de.FirstOrDefault().DateTime + de.FirstOrDefault().Open.ToString()); ;
        //}
        var finehub = new FinehubCrypoApi(candleRepo);
        var skender = new StockStrategy(alertService);
        var ma = new MAStrategy();

        //  var btc = new CryptoGetPrice<CryptoType.Gala>(finehub);
        //  var subscriber = new Subscriber();
        //   var btcStrategy = new BtcStrategy(repo , btc);

        //  btc.PriceChange30min();

        //var tester = new Strategytester();
        //var candles = candleRepo.GetCandles(CryptoType.bnb, ResolutionType._1h, DateTime.Now.AddDays(-15), DateTime.Now).ToList();
        //var res = tester.strategyTest<StockStrategy>(CryptoType.bnb, candles, new List<Func<List<CandlePosition>, List<CandlePosition>>>() {
        //    ma.TPStrategy
        //}, new List<Func<List<CandlePosition>, List<CandlePosition>>>());

        if (IsSync == false)
        {
          Helper.LogTofile("syncing ...");
          finehub.SyncMarketData(Enum.GetValues<CryptoType>().ToList());
          IsSync = true;
        }
        else
        {
          Helper.LogTofile($"دریافت دیتا در ساعت " + DateTime.Now);
          //finehub.GetPrice30Min(
          //    new List<CryptoType> { CryptoType.Ada, CryptoType.btc, CryptoType.bnb, CryptoType.Matic, CryptoType.Sol, CryptoType.Xrp },
          //    skender.Strategy30min);
          finehub.GetPrice1Hour(Enum.GetValues<CryptoType>().ToList(), skender.Strategy1Hour);
        }
        //     IEnumerable<Quote> quotes = _candleRepository.GetCandles(cryptoType, resolution, from, to).Select(a => (Quote)a).ToList();
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
      // var ghas = new GhasedakSMSProvider();
      GhasedakSMSProvider.AlertString("وایساد");
      Helper.LogTofile("وایساد");
      _timer.Stop();
    }
  }
}
