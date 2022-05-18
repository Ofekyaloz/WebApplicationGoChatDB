using System.ComponentModel.DataAnnotations;
using WebApplicationGoChat.Models;

namespace WebApplicationGoChat.Models
{
    public class User
    {
        [Key]
        public string Username { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Email { get; set; }
        public string Nickname { get; set; }
        public string Photo { get; set; }
        public List<Contact> Contacts { get; set; }
    }
}
