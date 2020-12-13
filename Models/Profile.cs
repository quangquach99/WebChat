using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebChat.Models
{
    public class Profile
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string UserEducation { get; set; }
        public int UserRelationship { get; set; }
        public string UserFacebook { get; set; }
        public string UserInstagram { get; set; }
        public string UserTwitter { get; set; }
        public string UserYoutube { get; set; }
        public string UserAvatar { get; set; }
    }
}
