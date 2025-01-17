using DziennikASPDotNetMVC.Models;
using DziennikASPDotNetMVC.Models.LinkTable;
using Microsoft.EntityFrameworkCore;

namespace DziennikASPDotNetMVC.Models
{
    public class MyDbContext:DbContext
    {
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Mail> Mails { get; set; }
        public DbSet<StudentClass> StudentClasses { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<HoursForLessons> HoursForLessons { get; set; }

        //Łącznikowe
        public DbSet<StudentWithClass> StudentWithClasses { get; set; }
        public DbSet<TeacherWithSubject> TeacherWithSubjects { get; set; }


        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Konfiguracja relacji 1:n między User a Mail
            modelBuilder.Entity<User>()
                .HasMany(u => u.messages)  // User ma wiele Mail
                .WithOne()  // Mail jest związany z jednym User
                .OnDelete(DeleteBehavior.Cascade);  // Usuń użytkownika, usuń wiadomości

            base.OnModelCreating(modelBuilder);
        }
    }
}
