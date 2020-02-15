using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UnleashedRage.Models
{
    public class User
    {
        
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Give us a unique name or something to go by")]
        [MaxLength(32, ErrorMessage = "Shorten that up a bit, it's too long")]
        [MinLength(3, ErrorMessage = "Needs to be a little longer then that")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Give us a password to protect your account with"), DataType(DataType.Password)]
        [MinLength(7, ErrorMessage = "Something longer than that")]
        public string Password { get; set; }

        [Required(ErrorMessage = "We need an email")] 
        [DataType(DataType.EmailAddress, ErrorMessage = "I don't think thats an email")]
        public string Email { get; set; }

        [Required]
        public bool SendEmail { get; set; }

        public string CurrPage { get; set; }

        public User() {
            Username = "User";
            Password = "12345";
            Email = "Email@email.com";
            CurrPage = "";
        }
        public User(string name) {
            Username = name;
            Password = "12345";
            Email = "Email@email.com";
            CurrPage = "";
        }
    }
}
