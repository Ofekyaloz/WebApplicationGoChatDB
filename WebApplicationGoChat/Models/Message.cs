using System.ComponentModel.DataAnnotations;

namespace WebApplicationGoChat.Models
{
    public class Message
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        public string sender { get; set; }
    }
}
