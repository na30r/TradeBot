using Microsoft.EntityFrameworkCore;
using Quartz;
using TradeBot.Models;
using TradeBot.Models.Enum;
using TradeBot.Repositories;
using TradeBot.Services;
using TradeBotV2.Models;

namespace TradeBotV2
{
    public class HelloJob : IJob
    {
        private FinehubCrypoApi _fineHub;
        private readonly GhasedakSMSProvider _smsProvider;
        private SkenderIndicators _skenderIndicators { get; }
        private CandleRepository _candleRepository { get; }
        private ApplicationDbContext _context{ get; }
        public HelloJob()
        {
            //var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            //optionBuilder.UseSqlServer("Server=.; Initial Catalog=TradeBot; Integrated Security=True");
            //using ApplicationDbContext ctx = new ApplicationDbContext(optionBuilder.Options);
            
            //var repo = new CandleRepository(ctx);
            //var finehub = new FinehubCrypoApi(repo);
            //var ghas = new GhasedakSMSProvider();
            //var skender = new SkenderIndicators(ctx, repo, ghas);
            //_context = ctx;
            //_fineHub = finehub;
            //_smsProvider = ghas;
            //_skenderIndicators = skender;
            //_candleRepository = repo;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                _smsProvider.AlertString($"دریافت دیتا در ساعت {DateTime.Now.TimeOfDay}");
                var s = _fineHub.GetPrice30Min(TradeBot.Models.Enum.CryptoType.Ada);
                var sd = _fineHub.GetPrice30Min(TradeBot.Models.Enum.CryptoType.btc);
                _skenderIndicators.StochStrategy30min(CryptoType.btc, DateTime.Now.AddDays(-2), DateTime.Now);
                _skenderIndicators.StochStrategy30min(CryptoType.Ada, DateTime.Now.AddDays(-2), DateTime.Now);
            }
            catch (Exception e)
            {
                try
                {
                    var myException = new MyException();
                    myException.Message = e.Message;
                    myException.InnerException = e.InnerException?.Message;
                    myException.StackTrace = e.StackTrace;
                    myException.DateTime = DateTime.Now;
                    _context.Add(myException);
                    _context.SaveChanges();
                }
                catch (Exception ee )
                {

                    throw;
                }
                _smsProvider.AlertString(e.Message?.Take(50).ToString());
            } 
        }
    }
}
