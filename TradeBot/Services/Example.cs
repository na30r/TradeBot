using System.Timers;
namespace TradeBot.Services
{
    public static class Example
    {
        private static System.Timers.Timer aTimer;

        private static System.Timers.Timer starterTimer;

        public static void Fire(int second, System.Timers.ElapsedEventHandler handler)
        {
            // Create a timer and set a two second interval.
            aTimer = new System.Timers.Timer();
            aTimer.Interval = 30 * 60 * 1000;
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += handler;

            // Have the timer fire repeated events (true is the default)
            aTimer.AutoReset = true;

            // Start the timer
            aTimer.Enabled = true;

            Console.ReadLine();
        }
        public static void Starter(timerTest timerTest)
        {
            var now = DateTime.Now;
            var startingTime = new DateTime(now.Year, now.Month, now.Day, now.Hour+1, 1, 0);
            var difference = (startingTime - now);
            var x = difference.TotalMilliseconds;

            // Create a timer and set a two second interval.
            aTimer = new System.Timers.Timer();
            aTimer.Interval = x;
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += timerTest.SendStartMessage;

            // Have the timer fire repeated events (true is the default)
            aTimer.AutoReset = false;

            // Start the timer
            aTimer.Enabled = true;

         //   Console.ReadLine();
        }
    }
}
