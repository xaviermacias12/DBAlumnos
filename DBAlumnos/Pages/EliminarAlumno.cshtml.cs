using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DBAlumnos.Data;
using DBAlumnos.Models;
using System.Threading.Tasks;
using System.IO;

namespace DBAlumnos.Pages
{
    public class EliminarAlumnoModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EliminarAlumnoModel(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public Alumno Alumno { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Alumno = await _context.Alumnos
                .FirstOrDefaultAsync(a => a.Id == id);

            if (Alumno == null)
            {
                TempData["ErrorMessage"] = "Alumno no encontrado";
                return RedirectToPage("/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Obtener el alumno con sus relaciones
            var alumno = await _context.Alumnos
                .Include(a => a.AlumnoMaterias)
                .FirstOrDefaultAsync(a => a.Id == Alumno.Id);

            if (alumno == null)
            {
                TempData["ErrorMessage"] = "Alumno no encontrado";
                return RedirectToPage("/Index");
            }

            // Verificar si tiene relaciones con materias
            if (alumno.AlumnoMaterias != null && alumno.AlumnoMaterias.Count > 0)
            {
                // Eliminar las relaciones primero
                _context.AlumnoMaterias.RemoveRange(alumno.AlumnoMaterias);
                await _context.SaveChangesAsync();
            }

            // Eliminar la foto del servidor si existe
            if (!string.IsNullOrEmpty(alumno.FotoUrl))
            {
                var rutaFoto = Path.Combine(_webHostEnvironment.WebRootPath,
                    alumno.FotoUrl.TrimStart('/'));
                if (System.IO.File.Exists(rutaFoto))
                {
                    System.IO.File.Delete(rutaFoto);
                }
            }

            // Eliminar el alumno
            _context.Alumnos.Remove(alumno);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Alumno '{alumno.Nombre}' eliminado con éxito";
            return RedirectToPage("/Index");
        }
    }
}