using System.ComponentModel.DataAnnotations;

namespace TradeBot.Domain.Models
{
    public class MyException
    {
        [Key]
        public int ID { get; set; }

        public DateTime DateTime { get; set; }
        public string InnerException { get; set; }
        public string StackTrace { get; set; }

        public string Message { get; set; }
    }
}
