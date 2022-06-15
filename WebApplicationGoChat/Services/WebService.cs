using Microsoft.EntityFrameworkCore;
using WebApplicationGoChat.Data;
using WebApplicationGoChat.Models;

namespace WebApplicationGoChat.Services
{
    public class WebService : IWebService
    {
        private readonly WebApplicationGoChatContext _context;
        public WebService(WebApplicationGoChatContext context)
        {
            _context = context;
        }

        public async Task<List<User>> getUsers()
        {
            return await _context.User.ToListAsync();
        }

        public async Task<User> getUser(string username)
        {
            var user = await _context.User.FindAsync(username);

            if (user == null)
            {
                return null;
            }

            return user;
        }

        public async void addUser(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<Contact>> getContacts(string username)
        {
            User user = await getUser(username);
            if(user == null)
            {
                return null;
            }
            
            await _context.Entry(user).Collection(e => e.Contacts).LoadAsync();
            return user.Contacts;
        }

        public async Task<Contact> getContact(string username, string contactname)
        {
            var contacts = _context.Contact.Where(e => e.userid == username && e.id == contactname);
            
            return await contacts.FirstOrDefaultAsync();
        }

        public async void addContact(string username, AddContactFields contactFields)
        {
            User user = await getUser(username);
            List<User> users = await getUsers();
            ICollection<Contact> contacts = user.Contacts;

            if (contacts.FirstOrDefault(m => m.id == contactFields.id) != null)
                return;

            if (users.Find(m => m.Username == contactFields.id) == null)
                return;
            
            user.Contacts.Add(new Contact { id = contactFields.id, name = contactFields.name, server = contactFields.server});
            await _context.SaveChangesAsync();
        }

        public async void editContact(string username, string id, UpdateContactFields contactFields)
        {
            Contact contact = await getContact(username, id);
            if (contact == null)
            {
                return;
            }
            contact.server = contactFields.server;
            contact.name = contactFields.name;
            await _context.SaveChangesAsync();
        }

        public async void removeContact(string username, string contactname)
        {
            ICollection<Contact> contacts = getContacts(username).Result;
            if (contacts == null)
            {
                return;
            }
            contacts.Remove(await getContact(username, contactname));
            await _context.SaveChangesAsync();
        }

        public async Task<List<Message>> getMessasges(string username, string contactname)
        {
            Contact contact = await getContact(username, contactname);
            await _context.Entry(contact).Collection(e => e.Messages).LoadAsync();
            if (contact == null)
            {
                return null;
            }

            return contact.Messages.ToList();
        }

        public async Task<Message> getMessasge(string username, string contactname, int id)
        {
            Message message = (await getMessasges(username, contactname)).Find(m => m.id == id);
            return message;
        }

        public async Task addMessage(string username, string contactname, string content, bool sender)
        {
            Contact contact = await getContact(username, contactname);
            if (contact == null)
            {
                return;
            }

            Message message = new Message() { 
                sent = sender, created = DateTime.Now.ToString(), content = content
                
            };
            contact.Messages.Add(message);
            contact.last = message.content;
            contact.lastdate = message.created;
            await _context.SaveChangesAsync();
        }

        public async void editMessage(string username, string contactname, int id, string content)
        {
            Contact contact = await getContact(username, contactname);
            if (contact == null)
            {
                return;
            }

            Message m = contact.Messages.FirstOrDefault(m => m.id == id);

            if (m != null)
            {
                m.content = content;
                await _context.SaveChangesAsync();
            }
        }
        public async void removeMessage(string username, string contactname, int id)
        {
            List<Message> messages = await getMessasges(username, contactname);
            if (messages == null)
            {
                return;
            }

            messages.Remove(await getMessasge(username, contactname, id));
            await _context.SaveChangesAsync();
        }
    }
}
