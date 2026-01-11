using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace Michalski.ComputerPheripherals.DAOSQL
{
    public class PeripheralsDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Tworzymy absolutną ścieżkę do pliku bazy danych w tym samym folderze,
            // w którym znajduje się plik DLL biblioteki. Gwarantuje to, że aplikacja
            // i narzędzia EF używają tego samego pliku.
            var dbPath = Path.Combine(AppContext.BaseDirectory, "computer_peripherals.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
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
