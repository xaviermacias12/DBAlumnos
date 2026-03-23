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
        public DbSet<Materia> Materias { get; set; }
        public DbSet<AlumnoMateria> AlumnoMaterias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ✅ Definir clave compuesta usando las propiedades de clave foránea
            modelBuilder.Entity<AlumnoMateria>()
                .HasKey(am => new { am.AlumnoId, am.MateriaId });

            // ✅ Configurar relación con Alumno
            modelBuilder.Entity<AlumnoMateria>()
                .HasOne(am => am.Alumno)
                .WithMany(a => a.AlumnoMaterias)
                .HasForeignKey(am => am.AlumnoId);

            // ✅ Configurar relación con Materia
            modelBuilder.Entity<AlumnoMateria>()
                .HasOne(am => am.Materia)
                .WithMany(m => m.AlumnoMaterias)
                .HasForeignKey(am => am.MateriaId); 

            base.OnModelCreating(modelBuilder);
        }
    }
}
