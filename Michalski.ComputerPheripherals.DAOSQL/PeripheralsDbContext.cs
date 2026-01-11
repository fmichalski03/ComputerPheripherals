using Microsoft.EntityFrameworkCore;

namespace Michalski.ComputerPheripherals.DAOSQL
{
    public class PeripheralsDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Definiujemy, że będziemy korzystać z bazy danych SQLite i podajemy nazwę pliku bazy
            optionsBuilder.UseSqlite("Data Source=computer_peripherals.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Konfiguracja relacji jeden-do-wielu: jeden producent może mieć wiele produktów
            modelBuilder.Entity<Manufacturer>()
                .HasMany(m => m.Products)
                .WithOne(p => p.Manufacturer)
                .HasForeignKey(p => p.ManufacturerId);
        }
    }
}
