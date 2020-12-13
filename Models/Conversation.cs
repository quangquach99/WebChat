using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebChat.Models
{
    public class Conversation
    {
        public int ID { get; set; }
        public int ConversationType { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<UserConversation> UserConversations { get; set; }
        public ICollection<MessageConversation> MessageConversations { get; set; }
    }
}
