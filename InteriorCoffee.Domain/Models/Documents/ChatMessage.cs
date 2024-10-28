using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Domain.Models.Documents
{
    public class ChatMessage
    {
        public string _id { get; set; }
        public string Sender { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
