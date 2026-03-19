using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DBAlumnos.Data;
using DBAlumnos.Models;
using System.Linq;

namespace DBAlumnos.Pages
{
    public class EliminarAlumnoModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public Alumno Alumno { get; set; }

        public EliminarAlumnoModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet(int id)
        {
            Alumno = _context.Alumnos.FirstOrDefault(a => a.Id == id);
        }

        public IActionResult OnPost()
        {
            _context.Alumnos.Remove(Alumno);
            _context.SaveChanges();

            return RedirectToPage("Index");
        }
    }
}