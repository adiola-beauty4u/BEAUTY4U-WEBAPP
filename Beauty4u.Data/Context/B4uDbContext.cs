using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty4u.Data.Context
{
    public class B4uDbContext : DbContext
    {
        public B4uDbContext(DbContextOptions<B4uDbContext> options)
        : base(options)
        {
        }

        //// Optional: define model for SP result
        //public DbSet<CustomerReportDto> CustomerReports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<CustomerReportDto>().HasNoKey(); // Important for SP results
        }
    }
}
