using Microsoft.EntityFrameworkCore;
using WebApiMagazin.Model;

namespace WebApiMagazin.Data
{
    public class ContextDb: DbContext
    {
        public DbSet<Product> Product { get; set; } = null!;
        public DbSet<Order> Order { get; set; } = null!;
        public DbSet<CreditCard> CreditCard { get; set; } = null!;
        public DbSet<Client> Client { get; set; } = null !;
        public DbSet<Transaction> Transaction { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=mydb.db");
            base.OnConfiguring(optionsBuilder);
        }


    }
}
