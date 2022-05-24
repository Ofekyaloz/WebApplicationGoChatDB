using WebApplicationGoChat.Models;

namespace WebApplicationGoChat.Services
{
    public interface IWebService
    {
        public List<User> getUsers();
        public User getUser(string username);
        public void addUser(User user);
        public List<Contact> getContacts(string username);
        public Contact getContact(string username, string contactname);
        public void addContact(string username, AddContactFields contactFields);
        public void editContact(string username, string id, UpdateContactFields contactFields);
        public void removeContact(string username, string contactname);
        public List<Message> getMessasges(string username, string contactname);
        public Message getMessasge(string username, string contactname, int id);
        public void addMessage(string username, string contactname, string content, bool sender);
        public void editMessage(string username, string contactname, int id, string content);
        public void removeMessage(string username, string contactname, int id);
    }
}
