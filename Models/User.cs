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

        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$",
             ErrorMessage = "Characters are not allowed.")]
        [Display(Name = "First Name"), Required]
        public string FirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$",
             ErrorMessage = "Characters are not allowed.")]
        [Display(Name = "Last Name"), Required]
        public string LastName { get; set; }

        // Get Full Name
        public string FullName
        {
            get
            {
                return LastName + " " + FirstName;
            }
        }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong mobile")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        [Required]
        public string Email { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9]{8,32}$", ErrorMessage = "8 to 32 characters, alphabets and numbers only")]
        [DataType(DataType.Password), Required]
        public string Password { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Of Birth"), Required]
        public DateTime DOB { get; set; }
        [Required(ErrorMessage = "Gender Can Not Be Empty!")]
        public int Gender { get; set; }
        public DateTime JoinedDate { 
            get
            {
                return System.DateTime.Now;
            }
        }
        public Profile Profile { get; set; }
        public ICollection<UserConversation> UserConversations { get; set; }
    }
}
