using ADO_EF_29._08._2023_1_.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO_EF_29._08._2023_1_.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Entity.Department> Departments { get; set; }
        public DbSet<Entity.Manager> Managers { get; set; }
        public DataContext() : base()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(
                    @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=ADO-EF-29.08.2023(1);Integrated Security=True"
            );
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Manager>()
                .HasOne(m => m.MainDep)
                .WithMany(d => d.MainManagers)
                .HasForeignKey(m => m.IdMainDep)
                .HasPrincipalKey(d => d.Id);
            modelBuilder
                .Entity<Manager>()
                .HasOne(m => m.SecDep)
                .WithMany()
                .HasForeignKey(m => m.IdSecDep)
                .HasPrincipalKey(m => m.Id);

            modelBuilder
                .Entity<Department>()
                .HasMany(d => d.SecManagers)
                .WithOne()
                .HasForeignKey(m => m.IdSecDep);


            modelBuilder
                .Entity<Manager>()
                .HasIndex(m => m.Login)
                .IsUnique();
            modelBuilder
                .Entity<Manager>()
                .Property(m => m.Name)
                .HasMaxLength(100);
            


        }
    }
}
