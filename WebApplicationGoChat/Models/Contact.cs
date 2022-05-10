using System.ComponentModel.DataAnnotations;

namespace WebApplicationGoChat.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        public List<Message> Messages { get; set; }
    }
}
