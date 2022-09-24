// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Topshelf;
using WindowsServiceV1;
//using Microsoft.AspNetCore.Builder;

//var builder = WebApplication.CreateBuilder(args);
//var cnnString = builder.Configuration.GetConnectionString("TradeBot");
//builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(cnnString));
//builder.Services.AddScoped<FinehubCrypoApi, FinehubCrypoApi>();
//builder.Services.AddScoped<CandleRepository, CandleRepository>();
//builder.Services.AddScoped<GhasedakSMSProvider, GhasedakSMSProvider>();
//builder.Services.AddScoped<timerTest, timerTest>();
//builder.Services.AddScoped<SkenderIndicators, SkenderIndicators>();

var exitCode = HostFactory.Run(x =>
{
    x.Service<Heartbeat>(s =>
  {
      s.ConstructUsing(x => new Heartbeat());
      s.WhenStarted(x => x.start());
      s.WhenStopped(a => a.Stop());
  });
    x.RunAsLocalSystem();
    x.SetServiceName("NasirV14");
    x.SetDisplayName("NasirV14");
});
//using IHost host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
//    .ConfigureServices(services =>
//    {
//        services.AddSingleton<JokeService>();
//        services.AddHostedService<WindowsBackgroundService>();
//    })
//    .Build();

//await host.RunAsync();
var exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
Environment.ExitCode = exitCodeValue;