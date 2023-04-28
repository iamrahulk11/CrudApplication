using CrudApplicationTest.Models;
using Microsoft.EntityFrameworkCore;

namespace CrudApplicationTest.Data
{
    public class ApplicationContext : DbContext
    {
//#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ApplicationContext(DbContextOptions options) : base(options) { }
//#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DbSet<Student> Students { get; set; }
    }
}
