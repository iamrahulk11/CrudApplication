using CrudApplicationTest.Models;
using Microsoft.EntityFrameworkCore;

namespace CrudApplicationTest.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options) { }
        public DbSet<Student> Students { get; set; }
    }
}
