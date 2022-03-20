using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using TradeBot.Models.Enum;

namespace TradeBot.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Candle> Candles { get; set; }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

    }




}
