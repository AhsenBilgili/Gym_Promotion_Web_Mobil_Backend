using DenemeForeignKey.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DenemeForeignKey.Data
{
    public class DataContext: IdentityDbContext<User>
    {
        public DbSet<Facility> Facilities { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
   
       

        public DbSet<SpecialCourse>SpecialCourses { get; set; }

        public DbSet<Trainer> Trainers { get; set; }

        public DbSet<DaySchedule> DaySchedules { get; set; }

        public DbSet<Course> Courses { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<CoursePrice> CoursePrices { get; set; }

        public DbSet<HomePageCard> HomePageCards { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(

                new IdentityRole { Name="Member", NormalizedName="MEMBER"},
                new IdentityRole { Name="Admin", NormalizedName="ADMIN"}
                );
        }
    }
}
