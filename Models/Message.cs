using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebChat.Models
{
    public class Message
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int ConversationID { get; set; }
        public string MessageText { get; set; }
        public DateTime SentTime { get; set; }
    }
}
