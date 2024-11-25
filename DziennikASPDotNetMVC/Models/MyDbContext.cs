using DziennikASPDotNetMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace DziennikASPDotNetMVC.Models
{
    public class MyDbContext:DbContext
    {
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Mail> Mails { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentClass> StudentClasses { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<ClassSchedule> ClassSchedules { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relacja 1:1 między Admin a User z określeniem klucza obcego
            modelBuilder.Entity<Admin>()
                .HasOne(a => a.user)  // Admin ma jedno User
                .WithOne()  // User ma jedno Admin
                .HasForeignKey<Admin>(a => a.adminId) // Określenie klucza obcego
                .OnDelete(DeleteBehavior.Cascade);  // Usuń użytkownika, usuń Admin

            // Relacja 1:1 między Teacher a User z określeniem klucza obcego
            modelBuilder.Entity<Teacher>()
                .HasOne(t => t.user)  // Teacher ma jedno User
                .WithOne()  // User ma jedno Teacher
                .HasForeignKey<Teacher>(t => t.teacherId)  // Określenie klucza obcego
                .OnDelete(DeleteBehavior.Cascade);  // Usuń użytkownika, usuń Teacher

            // Relacja 1:1 między Parent a User z określeniem klucza obcego
            modelBuilder.Entity<Parent>()
                .HasOne(p => p.user)  // Parent ma jedno User
                .WithOne()  // User ma jedno Parent
                .HasForeignKey<Parent>(p => p.parentId)  // Określenie klucza obcego
                .OnDelete(DeleteBehavior.Cascade);  // Usuń użytkownika, usuń Parent

            // Relacja 1:1 między Student a User z określeniem klucza obcego
            modelBuilder.Entity<Student>()
                .HasOne(s => s.user)  // Student ma jedno User
                .WithOne()  // User ma jedno Student
                .HasForeignKey<Student>(s => s.studentId)  // Określenie klucza obcego
                .OnDelete(DeleteBehavior.Cascade);  // Usuń użytkownika, usuń Student

            // Konfiguracja relacji 1:n między User a Mail
            modelBuilder.Entity<User>()
                .HasMany(u => u.messages)  // User ma wiele Mail
                .WithOne()  // Mail jest związany z jednym User
                .OnDelete(DeleteBehavior.Cascade);  // Usuń użytkownika, usuń wiadomości

            base.OnModelCreating(modelBuilder);
        }
    }
}
