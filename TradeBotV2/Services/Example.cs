using System.Timers;
namespace TradeBot.Services
{
    public static class Example
    {
        private static System.Timers.Timer aTimer;

        private static System.Timers.Timer starterTimer;

        public static void Fire(ElapsedEventHandler handler)
        {
            // Create a timer and set a two second interval.
            aTimer = new System.Timers.Timer();
           // aTimer.Interval = 30 * 60 * 1000;
            aTimer.Interval = 5000;
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += handler;

            // Have the timer fire repeated events (true is the default)
            aTimer.AutoReset = false;

            // Start the timer
            aTimer.Enabled = true;

        }
        public static void Starter(timerTest timerTest , GhasedakSMSProvider _SMSProvider)
        {
            var now = DateTime.Now;
            DateTime startingTime;
            if (now.Hour == 23)
            {
                 startingTime = new DateTime(now.Year, now.Month, now.Day+1, 0, 1, 0);
            }
            else
            {
                 startingTime = new DateTime(now.Year, now.Month, now.Day, now.Hour+1, 1, 0);
            }  
            var difference = (startingTime - now);
            _SMSProvider.AlertString($"برنامه ریزی برای {difference.TotalMinutes} دقیقه دیگر");
            var x = difference.TotalMilliseconds;

            // Create a timer and set a two second interval.
            aTimer = new System.Timers.Timer();
            //  aTimer.Interval = x;

            aTimer.Interval = 10000;
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
