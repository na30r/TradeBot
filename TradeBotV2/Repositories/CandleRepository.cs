﻿using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using TradeBot.Models;
using TradeBot.Models.Enum;
using TradeBotV2;
using static System.Net.Mime.MediaTypeNames;

namespace TradeBot.Repositories
{
    public class CandleRepository 
    {
        private  ApplicationDbContext _context;

        public CandleRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public bool IsExisted(Candle candle)
        {
            try
            {
               return _context.Candles.Any(a => a.DateTime == candle.DateTime && a.CoinName == candle.CoinName);
            }
            catch (Exception e )
            {
                var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionBuilder.UseSqlServer("Server=.; Initial Catalog=TradeBot; Integrated Security=True");
                using ApplicationDbContext ctx = new ApplicationDbContext(optionBuilder.Options);
                return ctx.Candles.Any(a => a.DateTime == candle.DateTime && a.CoinName == candle.CoinName);
            }
            throw new Exception(); 
        }

        public int InsertCandle(Candle candle)
        {

            var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionBuilder.UseSqlServer("Server=.; Initial Catalog=TradeBot; Integrated Security=True");
            using ApplicationDbContext ctx = new ApplicationDbContext(optionBuilder.Options);
            if (IsExisted(candle) == false)
            {
                _context.Add(candle);
                return _context.SaveChanges();
            }
            return 0;
        }

        public int InsertCandles(List<Candle> candles)
        {
            List<Candle> insertCandles = new List<Candle>();
            foreach (var candle in candles)
            {
                if (IsExisted(candle) == false)
                {
                    insertCandles.Add(candle);
                }
            }
            try
            {
                _context.AddRange(insertCandles);
                return _context.SaveChanges();
            }
            catch (Exception e)
            {
                var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionBuilder.UseSqlServer("Server=.; Initial Catalog=TradeBot; Integrated Security=True");
                using ApplicationDbContext ctx = new ApplicationDbContext(optionBuilder.Options);
                ctx.AddRange(insertCandles);
                return ctx.SaveChanges();
            }

        }

        public List<Candle> GetCandles(CryptoType cryptoType,ResolutionType resolution , DateTime from , DateTime to)
        {
            var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionBuilder.UseSqlServer("Server=.; Initial Catalog=TradeBot; Integrated Security=True");
            using ApplicationDbContext ctx = new ApplicationDbContext(optionBuilder.Options);
            var candles = _context.Candles
                .Where(a=>a.DateTime > from && a.DateTime<to && a.CoinName == (int) cryptoType)
                .Where(a=>a.Time % Resolution.ResolutionTimeDifference[resolution] == 0)
                .ToList();
            return candles;
        }
        public bool IsSync(Candle lastCandle)
        {
            if (DateTime.Now.Subtract(lastCandle.DateTime) < TimeSpan.FromMinutes(30))
            {
                return true;
            }else
            {
                return false;
            }
        }


        public Candle GetLastCandle(CryptoType cryptoName)
        {
           var candle = _context.Candles.Where(a => a.CoinName == (int)cryptoName).OrderByDescending(a => a.DateTime).FirstOrDefault();
           return candle;
        }


        public void Dispose()
        {
            
        }
    }
}
