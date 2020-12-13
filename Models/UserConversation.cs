using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebChat.Models
{
    public class UserConversation
    {
        public int ID { get; set; }
        public int ConversationID { get; set; }
        public int UserID { get; set; }
        public int UserSeen { get; set; }
        public User User { get; set; }
        public Conversation Conversation { get; set; }
    }
}
