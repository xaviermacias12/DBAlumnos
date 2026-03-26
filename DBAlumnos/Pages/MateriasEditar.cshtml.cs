using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DBAlumnos.Data;
using DBAlumnos.Models;
using System.Threading.Tasks;

namespace DBAlumnos.Pages
{
    public class MateriasEditarModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public MateriasEditarModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Materia Materia { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Materia = await _context.Materias.FindAsync(id);

            if (Materia == null)
            {
                TempData["ErrorMessage"] = "Materia no encontrada";
                return RedirectToPage("/MateriasIndex");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Error de validación. Verifica los campos.";
                return Page();
            }

            // Verificar si ya existe otra materia con el mismo nombre
            var existe = await _context.Materias
                .AnyAsync(m => m.Nombre == Materia.Nombre && m.Id != Materia.Id);

            if (existe)
            {
                ModelState.AddModelError("Materia.Nombre", "Ya existe otra materia con este nombre");
                TempData["ErrorMessage"] = "Ya existe otra materia con ese nombre";
                return Page();
            }

            try
            {
                _context.Attach(Materia).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Materia actualizada con éxito";
                return RedirectToPage("/MateriasIndex");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await MateriaExists(Materia.Id))
                {
                    TempData["ErrorMessage"] = "La materia ya no existe";
                    return RedirectToPage("/MateriasIndex");
                }
                throw;
            }
        }

        private async Task<bool> MateriaExists(int id)
        {
            return await _context.Materias.AnyAsync(m => m.Id == id);
        }
    }
}