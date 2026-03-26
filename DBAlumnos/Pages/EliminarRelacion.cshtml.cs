using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DBAlumnos.Data;
using DBAlumnos.Models;
using System.Threading.Tasks;

namespace DBAlumnos.Pages
{
    public class EliminarRelacionModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EliminarRelacionModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public AlumnoMateria Relacion { get; set; }

        public async Task<IActionResult> OnGetAsync(int alumnoId, int materiaId)
        {
            Relacion = await _context.AlumnoMaterias
                .Include(am => am.Alumno)
                .Include(am => am.Materia)
                .FirstOrDefaultAsync(am => am.AlumnoId == alumnoId && am.MateriaId == materiaId);

            if (Relacion == null)
            {
                TempData["ErrorMessage"] = "Relación no encontrada";
                return RedirectToPage("/AlumnoMaterias");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int alumnoId, int materiaId)
        {
            Relacion = await _context.AlumnoMaterias
                .FirstOrDefaultAsync(am => am.AlumnoId == alumnoId && am.MateriaId == materiaId);

            if (Relacion == null)
            {
                TempData["ErrorMessage"] = "Relación no encontrada";
                return RedirectToPage("/AlumnoMaterias");
            }

            try
            {
                _context.AlumnoMaterias.Remove(Relacion);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Relación eliminada con éxito";
                return RedirectToPage("/AlumnoMaterias");
            }
            catch (System.Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al eliminar: {ex.Message}";
                return RedirectToPage(new { alumnoId, materiaId });
            }
        }
    }
}