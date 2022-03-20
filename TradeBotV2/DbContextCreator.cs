using Microsoft.EntityFrameworkCore;
using TradeBot.Models;

namespace TradeBotV2
{
    public class DbContextCreator
    {
        public ApplicationDbContext GetInstance()
        {
            var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionBuilder.UseSqlServer("Server=.; Initial Catalog=TradeBot; Integrated Security=True");
            using ApplicationDbContext ctx = new ApplicationDbContext(optionBuilder.Options);
            return ctx;
        }
    }
}
