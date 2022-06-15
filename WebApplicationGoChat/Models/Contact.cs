using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace WebApplicationGoChat.Models
{
    public class Contact
    {
        public Contact()
        {
            Messages = new List<Message>();
        }

        [Key]
        public string id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string server { get; set; }
        public string last { get; set; }
        public string lastdate { get; set; }
        
        [JsonIgnore]
        public ICollection<Message> Messages { get; set; }
        [JsonIgnore]
        public User user { get; set; }
        [JsonIgnore]
        public string userid { get; set; }
    }
}
