using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DBAlumnos.Data;
using DBAlumnos.Models;
using System.Threading.Tasks;

namespace DBAlumnos.Pages
{
    public class MateriasCrearModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public MateriasCrearModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Materia Materia { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Error de validación. Verifica los campos.";
                return Page();
            }

            var existe = await _context.Materias
                .AnyAsync(m => m.Nombre == Materia.Nombre);

            if (existe)
            {
                ModelState.AddModelError("Materia.Nombre", "Ya existe una materia con este nombre");
                TempData["ErrorMessage"] = "Ya existe una materia con ese nombre";
                return Page();
            }

            try
            {
                _context.Materias.Add(Materia);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Materia creada con éxito";
                return RedirectToPage("/MateriasIndex");
            }
            catch (System.Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return Page();
            }
        }
    }
}