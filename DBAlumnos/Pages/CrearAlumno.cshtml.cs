using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DBAlumnos.Data;
using DBAlumnos.Models;

namespace DBAlumnos.Pages
{
    public class CrearAlumnoModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        [BindProperty]
        public Alumno Alumno { get; set; }
        public CrearAlumnoModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            _context.Alumnos.Add(Alumno);
            _context.SaveChanges();

            return RedirectToPage("Index");
        }
    }
}
