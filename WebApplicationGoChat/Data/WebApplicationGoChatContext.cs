#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApplicationGoChat.Models;

namespace WebApplicationGoChat.Data
{
    public class WebApplicationGoChatContext : DbContext
    {
        public WebApplicationGoChatContext (DbContextOptions<WebApplicationGoChatContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }

        public DbSet<Contact> Contact { get; set; }

        public DbSet<Message> Message { get; set; }
    }
}
