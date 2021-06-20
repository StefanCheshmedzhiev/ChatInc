using ChatInc.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChatInc.Data
{
    public class ChatIncDbContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users{ get; set; }

        public ChatIncDbContext(DbContextOptions options)
            : base(options)
        {

        }
    }
}
