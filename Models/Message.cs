using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebChat.Models
{
    public class Message
    {
        public int ID { get; set; }
        public string MessageText { get; set; }
        public DateTime SentTime { get; set; }
        public Sender Sender { get; set; }
        public ICollection<MessageConversation> MessageConversations { get; set; }
    }
}
