using Microsoft.EntityFrameworkCore;
using WebChat.Models;

namespace WebChat.Data
{
    public class WebChatContext : DbContext
    {
        public WebChatContext(DbContextOptions<WebChatContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<UserConversation> UserConversations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Sender> Senders { get; set; }
        public DbSet<MessageConversation> MessageConversations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Profile>().ToTable("Profile");
            modelBuilder.Entity<Conversation>().ToTable("Conversation");
            modelBuilder.Entity<UserConversation>().ToTable("UserConversation");
            modelBuilder.Entity<Message>().ToTable("Message");
            modelBuilder.Entity<Sender>().ToTable("Sender");
            modelBuilder.Entity<MessageConversation>().ToTable("MessageConversation");
        }
    }
}
