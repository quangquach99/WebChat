using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebChat.Models.WebChatViewModel
{
    public class MessagesViewModel
    {
        public int ConversationID { get; set; }
        public int SenderID { get; set; }
        public string SenderAvatar { get; set; }
        public string SenderMessage { get; set; }
        public DateTime MessagedTime { get; set; }
    }
}
