using WebApplicationGoChat.Models;

namespace WebApplicationGoChat.Services
{
    public interface IWebService
    {
        public Task<List<User>> getUsers();
        public Task<User> getUser(string username);
        public void addUser(User user);
        public Task<ICollection<Contact>> getContacts(string username);
        public Task<Contact> getContact(string username, string contactname);
        public Task addContact(string username, AddContactFields contactFields);
        public void editContact(string username, string id, UpdateContactFields contactFields);
        public void removeContact(string username, string contactname);
        public Task<List<Message>> getMessasges(string username, string contactname);
        public Task<Message> getMessasge(string username, string contactname, int id);
        public Task addMessage(string username, string contactname, string content, bool sender);
        public void editMessage(string username, string contactname, int id, string content);
        public void removeMessage(string username, string contactname, int id);
    }
}
