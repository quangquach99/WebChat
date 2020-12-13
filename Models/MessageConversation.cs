using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebChat.Models
{
    public class MessageConversation
    {
        public int ID { get; set; }
        public int MessageID { get; set; }
        public int ConversationID { get; set; }
        public Message Message { get; set; }
        public Conversation Conversation { get; set; }
    }
}
