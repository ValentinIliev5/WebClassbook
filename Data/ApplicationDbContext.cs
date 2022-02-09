using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WebClassbook.Models;

namespace WebClassbook.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<WebClassbook.Models.Subject> Subject { get; set; }
        public DbSet<WebClassbook.Models.Teacher> Teachers { get; set; }
        public DbSet<WebClassbook.Models.Admin> Admins { get; set; }
        public DbSet<WebClassbook.Models.Absence> Absences { get; set; }
        public DbSet<WebClassbook.Models.Exam> Exams { get; set; }
        public DbSet<WebClassbook.Models.Mark> Marks { get; set; }
        public DbSet<WebClassbook.Models.Remark> Remarks { get; set; }
    }
}
