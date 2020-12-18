using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebChat.Models
{
    public class Account
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        [Required(ErrorMessage = "Moi nhap Account!!!")]
        public string UserEmail { get; set; }
        [Required(ErrorMessage = "Moi nhap Pass!!!")]
        public string UserPassword { get; set; }
        public User User { get; set; }
    }
}
