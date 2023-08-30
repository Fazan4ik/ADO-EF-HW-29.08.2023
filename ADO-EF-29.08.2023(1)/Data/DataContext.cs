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
    }
}
