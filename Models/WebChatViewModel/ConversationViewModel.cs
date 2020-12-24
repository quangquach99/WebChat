using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebChat.Models.WebChatViewModel
{
    public class ConversationViewModel
    {
        public int ConversationID { get; set; }
        public string UserFullName { get; set; }
        public string UserAvatar { get; set; }
        public string LastMessage { get; set; }
        public string LastMessageTime { get; set; }
        public int IsSeen { get; set; }
    }
}
