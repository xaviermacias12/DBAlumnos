using Microsoft.EntityFrameworkCore;
using DBAlumnos.Models;
namespace DBAlumnos.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<Alumno> Alumnos { get; set; }
    }
}
