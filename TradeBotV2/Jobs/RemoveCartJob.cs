using Microsoft.EntityFrameworkCore;
using Quartz;
using TradeBot.Models;
using TradeBot.Models.Enum;
using TradeBot.Repositories;
using TradeBot.Services;

namespace TradeBotV2.Jobs
{
    [DisallowConcurrentExecution]
    public class RemoveCartJob : IJob
    {
        private FinehubCrypoApi _fineHub;
        public RemoveCartJob()
        {
            //var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            //optionBuilder.UseSqlServer("Server=.; Initial Catalog=TradeBot; Integrated Security=True");
            //using ApplicationDbContext _ctx = new ApplicationDbContext(optionBuilder.Options);
 
          
        }
        public Task Execute(IJobExecutionContext context)
        {
            var ghas = new GhasedakSMSProvider();
            var option = new DbContextOptionsBuilder<ApplicationDbContext>();
            option.UseSqlServer(@"Server=.; Initial Catalog=TradeBot; Integrated Security=True");
            using (ApplicationDbContext _ctx = new ApplicationDbContext(option.Options))
            {
                Console.WriteLine(DateTime.Now.ToString());
              //  ghas.AlertString(DateTime.Now.TimeOfDay.ToString());
                var repo = new CandleRepository(_ctx);
                var finehub = new FinehubCrypoApi(repo);
                var skender = new SkenderIndicators(_ctx, repo, ghas);
               
                //_smsProvider.AlertString($"دریافت دیتا در ساعت {DateTime.Now.TimeOfDay}");
                var s = finehub.GetPrice30Min(TradeBot.Models.Enum.CryptoType.Ada);
                var sd = finehub.GetPrice30Min(TradeBot.Models.Enum.CryptoType.btc);
                skender.StochStrategy30min(CryptoType.btc, DateTime.Now.AddDays(-2), DateTime.Now);
                skender.StochStrategy30min(CryptoType.Ada, DateTime.Now.AddDays(-2), DateTime.Now);
            }
            return Task.CompletedTask;
        }
    }
}
