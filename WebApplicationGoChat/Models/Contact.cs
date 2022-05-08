using System.ComponentModel.DataAnnotations;

namespace WebApplicationGoChat.Models
{
    public class Contact
    {
        [Key]
        public string Username { get; set; }
        [Required]
        public string Nickname { get; set; }
        public string Photo { get; set; }
    }
}
