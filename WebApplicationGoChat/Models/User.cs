using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
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
        [JsonIgnore]
        public ICollection<Contact> Contacts { get; set; }
        public string Connection { get; set; }
        public string Token { get; set; }
     
        public User()
        {
            Contacts = new List<Contact>();
        }
    }
}
