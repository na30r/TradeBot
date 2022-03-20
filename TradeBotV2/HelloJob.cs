using Quartz;

namespace TradeBotV2
{
    public class HelloJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await Console.Out.WriteLineAsync($"Greetings from HelloJob! {DateTime.Now}");
        }
    }
}
