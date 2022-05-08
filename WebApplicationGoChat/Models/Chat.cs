using System.ComponentModel.DataAnnotations;

namespace WebApplicationGoChat.Models
{
    public class Chat
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstContact { get; set; }
        [Required]
        public string SecondContact { get; set; }
        public List<Message> Messages { get; set; }
    }
}
