using Newtonsoft.Json;
using System.IO;
using System.Net.Http.Headers;
using TradeBot.Models;
using TradeBot.Services;
using Microsoft.EntityFrameworkCore;

using System.Timers;
using Ghasedak.Core.Interfaces;
using static System.Net.Mime.MediaTypeNames;
using TradeBot.Repositories;
using TradeBot.Models.Enum;

var builder = WebApplication.CreateBuilder(args);

var cnnString = builder.Configuration.GetConnectionString("TradeBot");
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(cnnString));
builder.Services.AddScoped<FinehubCrypoApi, FinehubCrypoApi>();
builder.Services.AddScoped<CandleRepository, CandleRepository>();
builder.Services.AddScoped<GhasedakSMSProvider, GhasedakSMSProvider>();
builder.Services.AddScoped<timerTest, timerTest>();
builder.Services.AddScoped<SkenderIndicators, SkenderIndicators>();



using (ServiceProvider serviceProvider = builder.Services.BuildServiceProvider())
{
    // Review the FormMain Singleton.
    var crypoApiService = serviceProvider.GetRequiredService<FinehubCrypoApi>();
    var smsProvider = serviceProvider.GetRequiredService<GhasedakSMSProvider>();
    var timerTest = serviceProvider.GetRequiredService<timerTest>();
    var skenderIndicators = serviceProvider.GetRequiredService<SkenderIndicators>();

    //  crypoApiService.GetPrice(CryptoType.btc,ResolutionType._30min,DateTime.Now.AddDays(-2),DateTime.Now);
    skenderIndicators.test(CryptoType.btc, ResolutionType._30min, DateTime.Now.AddDays(-3), DateTime.Now);
    var now = DateTime.Now;
    var startingTime = new DateTime(now.Year, now.Month, now.Day, now.Hour + 1, 1, 0);
    var difference = (startingTime - now).Ticks;

    Example.Starter(timerTest);
    //formMain.GetPrice(TradeBot.Models.Enum.CryptoType.btc, DateTime.Now.AddDays(-3), DateTime.Now);
}

var app = builder.Build();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Candles}/{action=Index}/{id?}");
app.Run();




