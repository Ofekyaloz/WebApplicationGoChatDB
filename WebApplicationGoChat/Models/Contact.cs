using System.ComponentModel.DataAnnotations;

namespace WebApplicationGoChat.Models
{
    public class Contact
    {
        [Key]
        public string id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string server { get; set; }
        public string? last { get; set; }
        public string? lastdate { get; set; }
    }
}
