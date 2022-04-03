//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Reflection.Emit;
//using TradeBot.Models.Enum;
//using TradeBotV2.Models;

//namespace TradeBot.Models
//{
//    public class ApplicationDbContext : DbContext
//    {
//        public DbSet<Candle> Candles { get; set; }
//        public DbSet<MyException> Exceptions{ get; set; }
//        //public ApplicationDbContext(DbContextOptions options) : base(options)
//        //{
//        //}
//        //Singletone constructor
//        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
//        {
//          //  AddExecutionStrategy(() => new SqlAzureExecutionStrategy());
//        }
//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {

//            base.OnConfiguring(optionsBuilder);
//        }
//    }
//}
