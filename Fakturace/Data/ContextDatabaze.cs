using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fakturace.Model;

namespace Fakturace.Data
{
    public class ContextDatabaze : DbContext
    {
        public DbSet<Dodavatel> Dodavatele { get; set; }
        public DbSet<Odberatel> Odberatele { get; set; }
        public DbSet<Faktura> PrijateFaktury { get; set; }
        public DbSet<Faktura> VystaveneFaktury { get; set; }

        public ContextDatabaze()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FakturaceVystavene.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }
}
