using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebChat.Models
{
    public class User
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }
        public int Gender { get; set; }
        public DateTime JoinedDate { get; set; }
        public Account Account { get; set; }
        public Profile Profile { get; set; }
        public Sender Sender { get; set; }
        public ICollection<UserConversation> UserConversations { get; set; }
    }
}
