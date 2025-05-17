using Microsoft.EntityFrameworkCore;
using TutorialProjectAPI.Models;

namespace TutorialProjectAPI.Contexts
{
    public class MainContext : DbContext
    {
        public MainContext(DbContextOptions<MainContext> options) : base(options)
        {
            //todo
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //todo
        }
        public DbSet<UserDB> Users { get; set; }
    }
}
