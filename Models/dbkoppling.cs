using Microsoft.EntityFrameworkCore;

namespace BasketForum.Models
{
    public class dbkoppling : DbContext
    {//skapar 2 tabeller Users och Inlagg 
        public DbSet<Users> Users { get; set; }
        public DbSet<Inlagg> Inlagg { get; set; }
        public DbSet<Kommentar> Kommentarer { get; set; }

        public dbkoppling()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("DataSource = databas.db"); //Databasfilens namn
        }
    }
}
