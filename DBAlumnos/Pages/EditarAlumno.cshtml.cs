using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DBAlumnos.Data;
using DBAlumnos.Models;
using System.Linq;

namespace DBAlumnos.Pages
{
    public class EditarAlumnoModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        [BindProperty]
        public Alumno Alumno { get; set; }

        public EditarAlumnoModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet( int id)
        {
            Alumno = _context.Alumnos.FirstOrDefault(a => a.Id == id);
        }

        public IActionResult OnPost()
        {
            _context.Alumnos.Update(Alumno);
            _context.SaveChanges();

            return RedirectToPage("Consulta");
        }
    }
}
