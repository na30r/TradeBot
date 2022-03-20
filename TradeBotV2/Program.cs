using Microsoft.EntityFrameworkCore;
using Quartz.Impl;
using Quartz;
using TradeBot.Models;
using TradeBot.Models.Enum;
using TradeBot.Repositories;
using TradeBot.Services;
using TradeBotV2.Repositories;
using Quartz.Logging;
using TradeBotV2;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var cnnString = builder.Configuration.GetConnectionString("TradeBot");
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(cnnString));
builder.Services.AddScoped<FinehubCrypoApi, FinehubCrypoApi>();
builder.Services.AddScoped<CandleRepository, CandleRepository>();
builder.Services.AddScoped<GhasedakSMSProvider, GhasedakSMSProvider>();
builder.Services.AddScoped<timerTest, timerTest>();
builder.Services.AddScoped<SkenderIndicators, SkenderIndicators>();

var app = builder.Build();


//using (ServiceProvider serviceProvider = builder.Services.BuildServiceProvider())
//{
//    var skenderIndicators = serviceProvider.GetRequiredService<SkenderIndicators>();
//    skenderIndicators.test(CryptoType.Ada, ResolutionType._30min, DateTime.Now.AddDays(-2), DateTime.Now);

//}

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//Example.Starter(_timertest, _smsProvider);


//quartz
LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());

// Grab the Scheduler instance from the Factory
StdSchedulerFactory factory = new StdSchedulerFactory();
IScheduler scheduler = await factory.GetScheduler();

// and start it off
await scheduler.Start();

// define the job and tie it to our HelloJob class
IJobDetail job = JobBuilder.Create<HelloJob>()
    .WithIdentity("job1", "group1")
    .Build();

ITrigger trigger = TriggerBuilder.Create()
    .WithIdentity("trigger3", "group1")
    .WithCronSchedule("0 1/30 * * * ?")
    .ForJob(job)
    .Build();

//ITrigger trigger = TriggerBuilder.Create()
//    .WithIdentity("trigger1", "group1")
//        .WithCronSchedule("*/53 * * * *")
//    .ForJob(job)
//    .Build();

// Tell quartz to schedule the job using our trigger
await scheduler.ScheduleJob(job, trigger);

// some sleep to show what's happening
//await Task.Delay(TimeSpan.FromSeconds(60));

// and last shut down the scheduler when you are ready to close your program
//await scheduler.Shutdown();

//Console.WriteLine("Press any key to close the application");
//Console.ReadKey();


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
