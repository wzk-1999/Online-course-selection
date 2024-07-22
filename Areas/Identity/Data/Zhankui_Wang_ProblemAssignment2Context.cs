using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Zhankui_Wang_ProblemAssignment2.Models;

namespace Zhankui_Wang_ProblemAssignment2.Data;

public class Zhankui_Wang_ProblemAssignment2Context : IdentityDbContext<IdentityUser>
{
    public Zhankui_Wang_ProblemAssignment2Context(DbContextOptions<Zhankui_Wang_ProblemAssignment2Context> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Configure the one-to-many relationship
        builder.Entity<Student>()
            .HasOne(s => s.Course)
            .WithMany(c => c.Students)
            .HasForeignKey(s => s.CourseID); // Foreign key in Student referring to CourseID

        // Configure enum to string conversion
        builder.Entity<Student>()
    .Property(s => s._Status)
    .HasConversion(
        v => v.ToString(),
        v => (Student.Status)Enum.Parse(typeof(Student.Status), v)
    );

        // Generate check constraint string dynamically
        var enumValues = string.Join(", ", Enum.GetNames(typeof(Student.Status)).Select(v => $"'{v}'"));
        var checkConstraint = $"_Status IN ({enumValues})";


        // Add check constraint for _Status

        builder.Entity<Student>().ToTable(t => t.HasCheckConstraint("CK_Students_Status", checkConstraint));

        // Seed data for Course
        builder.Entity<Course>().HasData(
            new Course
            {
                CourseID = 1,
                Code = "CSC101",
                Title = "Introduction to Computer Science",
                ClassRoom = "CA101",
                Instructor = "Dr. John Doe"
            },
            new Course
            {
                CourseID = 2,
                Code = "MAT201",
                Title = "Calculus I",
                ClassRoom = "CB201",
                Instructor = "Dr. Jane Smith"
            },
            new Course
            {
                CourseID = 3,
                Code = "PHY301",
                Title = "Physics I",
                ClassRoom = "AC301",
                Instructor = "Dr. Albert Einstein"
            }
        );

        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

public DbSet<Zhankui_Wang_ProblemAssignment2.Models.Course> Course { get; set; } = default!;
    public DbSet<Student> Students { get; set; }
}
