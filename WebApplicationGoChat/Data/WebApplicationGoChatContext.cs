#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplicationGoChat.Models;

namespace WebApplicationGoChat.Data
{
    public class WebApplicationGoChatContext : DbContext
    {
        public WebApplicationGoChatContext (DbContextOptions<WebApplicationGoChatContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>().HasKey(e => new {e.id, e.userid});
        }
        
        public DbSet<User> User { get; set; }

        public DbSet<Contact> Contact { get; set; }

        public DbSet<Message> Message { get; set; }
    }
}
