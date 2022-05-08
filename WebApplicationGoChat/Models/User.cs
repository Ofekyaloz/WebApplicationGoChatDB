using System.ComponentModel.DataAnnotations;

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
        public List<Chat> Chats { get; set; }


    }
}
