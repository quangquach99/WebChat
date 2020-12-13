using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebChat.Models
{
    public class Sender
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int MessageID { get; set; }
        public User User { get; set; }
        public Message Message { get; set; }
    }
}
