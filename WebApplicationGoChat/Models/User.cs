using System.ComponentModel.DataAnnotations;
using WebApplicationGoChat.Models;

namespace WebApplicationGoChat.Models
{
    public class User
    {
        [Key]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Nickname { get; set; }
        [Required]
        public string Photo { get; set; }
        public List<Contact> Contacts { get; set; }
    }
}
