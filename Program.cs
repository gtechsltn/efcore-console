/*
 * dotnet new console
 * dotnet add package Microsoft.EntityFrameworkCore.SqlServer
 * dotnet add package Microsoft.EntityFrameworkCore.Tools
 * dotnet ef migrations add CreateSchoolDB
 * dotnet ef database update
 *
 * Install-Package Microsoft.EntityFrameworkCore.SqlServer
 * Install-Package Microsoft.EntityFrameworkCore.Tools
 * add-migration CreateSchoolDB
 * update-database –verbose
 *
 * add-migration UpdateSchoolDB-Add-Team-Player
 * update-database –verbose
 *
 * Scaffold-DbContext "Server=localhost\SQLEXPRESS;Database=SchoolDB;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
 *
 * dotnet new console -o EFCoreDBContextApplication
 * cd EFCoreDBContextApplication
 * dotnet add package Microsoft.EntityFrameworkCore
 * dotnet add package Microsoft.EntityFrameworkCore.Tools
 * dotnet add package Microsoft.EntityFrameworkCore.SqlServer
 * dotnet add package Microsoft.Extensions.DependencyInjection
 * dotnet tool install --global dotnet-ef
 * dotnet ef dbcontext scaffold "Server=localhost\SQLEXPRESS;Database=SchoolDB;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models
 * dotnet run
 */

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EFCore
{
    /// <summary>
    /// https://www.entityframeworktutorial.net/efcore/entity-framework-core-console-application.aspx
    /// https://www.learnentityframeworkcore.com/walkthroughs/console-application
    /// http://www.techtutorhub.com/article/How-to-Add-Entity-Framework-Core-DBContext-in-Dot-NET-Core-Console-Application/86
    /// https://voltwu.github.io/blog/csharp-ef/2020/02/25/Tutorial-Explore-EF-Core-Console-Application/
    /// </summary>
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            using (var context = new SchoolContext())
            {
                //Create (C)
                if (!context.Students.Any())
                {
                    var student = new Student()
                    {
                        Name = "Bill"
                    };

                    context.Students.Add(student);
                    context.SaveChanges();
                }
            }

            //List all items
            using (var context = new SchoolContext())
            {
                foreach (var student in context.Students)
                {
                    Console.WriteLine(student.Name);
                }
            }

            //Update (U)
            using (var context = new SchoolContext())
            {
                Student std = context.Students.Find(1);
                if (std != null)
                {
                    std.Name = "Bill Gates";
                    context.SaveChanges();
                }
            }

            //Read (R)
            using (var context = new SchoolContext())
            {
                foreach (var student in context.Students)
                {
                    Console.WriteLine(student.Name);
                }
            }

            //Delete (D)
            using (var context = new SchoolContext())
            {
                Student std = context.Students.Find(1);
                if (std != null)
                {
                    context.Students.Remove(std);
                    context.SaveChanges();
                }
            }

            Console.Write("Press any key to exit...");
            Console.ReadKey();
        }
    }

    public class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
    }

    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
    }

    public class SchoolContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=SchoolDB;Uid=sa;Pwd=123456;MultipleActiveResultSets=true;");
        }

        public DbSet<Team> Teams { set; get; }
        public DbSet<Player> players { set; get; }
    }

    public class Team
    {
        [Key]
        [Required]
        public int Id { set; get; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(20, ErrorMessage = "Name is too long")]
        public string Name { set; get; }

        public List<Player> Players { set; get; }
    }

    public class Player
    {
        [Key]
        [Required]
        public int Id { set; get; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(20, ErrorMessage = "Name is too long")]
        public string Name { set; get; }
    }
}