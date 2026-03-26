using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DBAlumnos.Data;
using DBAlumnos.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBAlumnos.Pages
{
    public class MateriasEliminarModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public MateriasEliminarModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Materia Materia { get; set; }

        public bool TieneRelaciones { get; set; }

        public List<Alumno> AlumnosRelacionados { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Materia = await _context.Materias.FindAsync(id);

            if (Materia == null)
            {
                TempData["ErrorMessage"] = "Materia no encontrada";
                return RedirectToPage("/MateriasIndex");
            }

            // Obtener los alumnos relacionados con esta materia
            AlumnosRelacionados = await _context.AlumnoMaterias
                .Where(am => am.MateriaId == id)
                .Include(am => am.Alumno)
                .Select(am => am.Alumno)
                .ToListAsync();

            TieneRelaciones = AlumnosRelacionados.Any();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Materia = await _context.Materias.FindAsync(id);

            if (Materia == null)
            {
                TempData["ErrorMessage"] = "Materia no encontrada";
                return RedirectToPage("/MateriasIndex");
            }

            // Verificar nuevamente si tiene relaciones
            var tieneRelaciones = await _context.AlumnoMaterias
                .AnyAsync(am => am.MateriaId == id);

            if (tieneRelaciones)
            {
                TempData["ErrorMessage"] = "No se puede eliminar la materia porque tiene alumnos inscritos.";
                return RedirectToPage(new { id = id });
            }

            try
            {
                _context.Materias.Remove(Materia);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Materia '{Materia.Nombre}' eliminada con éxito";
                return RedirectToPage("/MateriasIndex");
            }
            catch (System.Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al eliminar: {ex.Message}";
                return RedirectToPage(new { id = id });
            }
        }
    }
}