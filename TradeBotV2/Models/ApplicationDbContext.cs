using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using TradeBot.Models.Enum;

namespace TradeBot.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Candle> Candles { get; set; }
        //public ApplicationDbContext(DbContextOptions options) : base(options)
        //{
        //}
        //Singletone constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            base.OnConfiguring(optionsBuilder);
        }
    }
}
