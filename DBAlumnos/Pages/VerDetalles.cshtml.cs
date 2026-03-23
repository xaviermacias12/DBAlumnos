using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DBAlumnos.Data;
using DBAlumnos.Models;
using Microsoft.EntityFrameworkCore;

namespace DBAlumnos.Pages
{
    public class VerDetallesModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public VerDetallesModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<AlumnoMateria> Lista { get; set; } = new();
        public Alumno AlumnoSeleccionado { get; set; } = new();
        public void OnGet(int id)
        {
            // Traer solo el alumno con ese id
            AlumnoSeleccionado = _context.Alumnos.FirstOrDefault(a => a.Id == id);
            // Traer solo las materias del alumno con ese id
            Lista = _context.AlumnoMaterias
                .Where(am => am.AlumnoId == id)
                .Include(am => am.Materia)
                .ToList();
        }
    }
}
