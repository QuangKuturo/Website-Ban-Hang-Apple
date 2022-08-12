using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TECH.Data.DatabaseEntity
{
    public class DataBaseEntityContext : DbContext
    {
        public DataBaseEntityContext(DbContextOptions<DataBaseEntityContext> options) : base(options) { }

        public DbSet<Users> users { set; get; }
        public DbSet<Contracts> contacts { set; get; }
        public DbSet<Category> categories { set; get; }
        public DbSet<Products> products { set; get; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany<Products>(g => g.Products)
                .WithOne(s => s.Category)
                .HasForeignKey(s=>s.category_id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
