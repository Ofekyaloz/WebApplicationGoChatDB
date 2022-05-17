using WebApplicationGoChat.Models;

namespace WebApplicationGoChat.Services
{
    public class WebService : IWebService
    {
        public static List<User> _users = new List<User>() { new User()
        {
            Username = "giligutfeld",
                Password = "123456",
                Email = "gili@gmail.com",
                Nickname = "Gili",
                Photo = "./Pictures/cat.jpg",
                Contacts = new List<Contact>() { new Contact() { id = "ofekyaloz", name = "Ofek", server = "localhost:7265", last = "Hello!", lastdate = "1-1-2022",
                Messages = new List<Message>() { new Message() { id = 1, content = "Hello!", created = "1-1-2022", sent = true } } },
                    new Contact() { id = "leomessi", name = "Leo", server = "localhost:7265", last = "Hi!", lastdate = "15-1-2022",
                Messages = new List<Message>() } }
            }, new User()
        {
            Username = "ofekyaloz",
                Password = "234567",
                Email = "ofek@gmail.com",
                Nickname = "Ofek",
                Photo = "./Pictures/cat.jpg",
                Contacts = new List<Contact>() { new Contact() { id = "giligutfeld", name = "Gili", server = "localhost:7265", last = "Hello!", lastdate = "1-1-2022",
                Messages = new List<Message>() { new Message() { id = 1, content = "Hello!", created = "1-1-2022", sent = false } } } }
            }, new User() { Username = "noakirel", Password = "111111", Email = "noa@gmail.com", Nickname = "Noa", Photo = "./Pictures/cat.jpg", Contacts = new List<Contact>() },
        new User() { Username = "omeradam", Password = "shigramefoeret", Email = "omer@gmail.com", Nickname = "Omer", Photo = "./Pictures/cat.jpg", Contacts = new List<Contact>() },
        new User() { Username = "bibinetanyahu", Password = "bbbbbb", Email = "bibi@gmail.com", Nickname = "Bibi", Photo = "./Pictures/cat.jpg", Contacts = new List<Contact>() },
        new User() { Username = "edenhason", Password = "shemishuyaazoroti", Email = "eden@gmail.com", Nickname = "Eden", Photo = "./Pictures/cat.jpg", Contacts = new List<Contact>() },
        new User() { Username = "leomessi", Password = "101010", Email = "leo@gmail.com", Nickname = "Messi", Photo = "./Pictures/cat.jpg", Contacts = new List<Contact>() }
    };

        public List<User> getUsers()
        {
            return _users;
        }

        public User getUser(string username)
        {
            User user = getUsers().Find(m => m.Username == username);

            if (user == null)
            {
                return null;
            }

            return user;
        }

        public void addUser(User user)
        {
            _users.Add(user);
        }

        public List<Contact> getContacts(string username)
        {
            if(getUser(username) == null)
            {
                return null;
            }

            return getUser(username).Contacts;
        }

        public Contact getContact(string username, string contactname)
        {
            if(getContacts(username) == null)
            {
                return null;
            }

            Contact contact = getContacts(username).Find(m => m.id == contactname);
            if (contact == null)
            {
                return null;
            }

            return contact;
        }

        public void addContact(string username, Contact contact)
        {
            User user = getUser(username);
            contact.Messages = new List<Message>();
            user.Contacts.Add(contact);
        }

        public void editContact(string username, Contact contact)
        {
            Contact x = getContact(username, contact.id);
            if (x == null)
            {
                return;
            }
            x.server = contact.server;
            x.name = contact.name;
        }

        public void removeContact(string username, string contactname)
        {
            List<Contact> contacts = getContacts(username);
            if (contacts == null)
            {
                return;
            }
            contacts.Remove(getContact(username, contactname));
        }

        public List<Message> getMessasges(string username, string contactname)
        {
            Contact contact = getContact(username, contactname);
            if (contact == null)
            {
                return null;
            }

            return contact.Messages;
        }

        public Message getMessasge(string username, string contactname, int id)
        {
            if (getMessasges(username, contactname) == null)
            {
                return null;
            }

            Message message = getMessasges(username, contactname).Find(m => m.id == id);
            if (message == null)
            {
                return null;
            }

            return message;
        }

        public void addMessage(string username, string contactname, Message message)
        {
            Contact contact = getContact(username, contactname);
            if (contact == null)
            {
                return;
            }

            contact.Messages.Add(message);
        }

        public void editMessage(string username, string contactname, Message message)
        {
            Contact contact = getContact(username, contactname);
            if (contact == null)
            {
                return;
            }

            Message m = contact.Messages.Find(m => m.id == message.id);

            if (m == null)
            {
                m.content = message.content;
            }
        }
        public void removeMessage(string username, string contactname, int id)
        {
            List<Message> messages = getMessasges(username, contactname);
            if (messages == null)
            {
                return;
            }

            messages.Remove(getMessasge(username, contactname, id));
        }
    }
}
