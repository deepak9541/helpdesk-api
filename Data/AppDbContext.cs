using Microsoft.EntityFrameworkCore;
using HelpdeskApi.Entities;
using HelpdeskApi.Enums;

namespace HelpdeskApi.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Ticket> Tickets { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.CreatedBy)
                .WithMany()
                .HasForeignKey(t => t.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.AssignedTo)
                .WithMany()
                .HasForeignKey(t => t.AssignedToId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Ticket>().Property(t => t.Status).HasConversion<string>();
            modelBuilder.Entity<Ticket>().Property(t => t.Priority).HasConversion<string>();
            modelBuilder.Entity<User>().Property(u => u.Role).HasConversion<string>();

            modelBuilder.Entity<User>().HasData(
   new User { Id = 1, FullName = "Admin User", Email = "admin@helpdesk.com", Role = Roles.Admin, PasswordHash = "$2a$11$hwhUhSevm2S09j51PTx1.OqSKNuU1ZT7HqVRmiCbAtDFZpF52mNmK" },
   new User { Id = 2, FullName = "Rahul Employee", Email = "rahul@helpdesk.com", Role = Roles.Employee, PasswordHash = "$2a$11$hwhUhSevm2S09j51PTx1.OqSKNuU1ZT7HqVRmiCbAtDFZpF52mNmK" },
   new User { Id = 3, FullName = "Priya Employee", Email = "priya@helpdesk.com", Role = Roles.Employee, PasswordHash = "$2a$11$hwhUhSevm2S09j51PTx1.OqSKNuU1ZT7HqVRmiCbAtDFZpF52mNmK" },
   new User { Id = 4, FullName = "Amit Developer", Email = "amit@helpdesk.com", Role = Roles.Developer, PasswordHash = "$2a$11$hwhUhSevm2S09j51PTx1.OqSKNuU1ZT7HqVRmiCbAtDFZpF52mNmK" },
   new User { Id = 5, FullName = "Neha Developer", Email = "neha@helpdesk.com", Role = Roles.Developer, PasswordHash = "$2a$11$hwhUhSevm2S09j51PTx1.OqSKNuU1ZT7HqVRmiCbAtDFZpF52mNmK" }
);
        }
    }
}
