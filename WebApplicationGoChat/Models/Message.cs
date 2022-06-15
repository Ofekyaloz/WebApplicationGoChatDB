using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApplicationGoChat.Models
{
    public class Message
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string content { get; set; }
        [Required]
        public string created { get; set; }
        [Required]
        public bool sent { get; set; }
        
        [JsonIgnore]
        public Contact contact { get; set; }
    }
}
