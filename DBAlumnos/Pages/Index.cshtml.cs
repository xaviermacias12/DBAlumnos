using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DBAlumnos.Data;
using DBAlumnos.Models;
using System.Collections.Generic;
using System.Linq;

namespace DBAlumnos.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public List<Alumno> ListaAlumnos { get; set; }
        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
            ListaAlumnos = _context.Alumnos.ToList();
        }
    }
}
