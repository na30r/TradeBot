﻿#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TradeBot.Models;
using TradeBot.Models.Enum;
using TradeBot.Models.ViewModels;
using TradeBot.Repositories;

namespace TradeBot.Controllers
{
    public class CandlesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly CandleRepository _candleRepository;

        public CandlesController(ApplicationDbContext context, CandleRepository candleRepository)
        {
            this._context = context;
            _candleRepository = candleRepository;
        }

        public IActionResult GetCandles()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetCandles(SearchCandleViewModel model)
        {
            var x = Request.Form;
            return View();
        }
        // GET: Candles

        public async Task<IActionResult> Index()
        {
            return View(await _context.Candles.ToListAsync());
        }

        // GET: Candles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candle = await _context.Candles
                .FirstOrDefaultAsync(m => m.ID == id);
            if (candle == null)
            {
                return NotFound();
            }

            return View(candle);
        }

        // GET: Candles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Candles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CoinName,High,Low,Open,Close,Time,v,DateTime")] Candle candle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(candle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(candle);
        }

        // GET: Candles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candle = await _context.Candles.FindAsync(id);
            if (candle == null)
            {
                return NotFound();
            }
            return View(candle);
        }

        // POST: Candles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CoinName,High,Low,Open,Close,Time,v,DateTime")] Candle candle)
        {
            if (id != candle.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(candle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CandleExists(candle.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(candle);
        }

        // GET: Candles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candle = await _context.Candles
                .FirstOrDefaultAsync(m => m.ID == id);
            if (candle == null)
            {
                return NotFound();
            }

            return View(candle);
        }

        // POST: Candles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var candle = await _context.Candles.FindAsync(id);
            _context.Candles.Remove(candle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CandleExists(int id)
        {
            return _context.Candles.Any(e => e.ID == id);
        }
    }
}
