using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DBAlumnos.Data;
using DBAlumnos.Models;
using Microsoft.EntityFrameworkCore;

namespace DBAlumnos.Pages
{
    public class VerInfoModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public VerInfoModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public Alumno AlumnoSeleccionado { get; set; }
        public void OnGet(int id)
        {
            AlumnoSeleccionado = _context.Alumnos.FirstOrDefault(a => a.Id == id);
        }
    }
}
