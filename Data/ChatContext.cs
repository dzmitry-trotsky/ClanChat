using GroupChat.Models;
using Microsoft.EntityFrameworkCore;

namespace GroupChat.Data
{
    public class ChatContext : DbContext
    {
        public ChatContext(DbContextOptions<ChatContext> options) : base(options)
        {
        }

        public DbSet<Clan> Clans { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ClanUser> ClanUsers { get; set; }
    }
}
