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
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Alumno> ListaAlumnos { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.Alumnos.AsQueryable();

            // Aplicar búsqueda si hay término
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                query = query.Where(a =>
                    a.Nombre.Contains(SearchTerm) ||
                    a.Carrera.Contains(SearchTerm) ||
                    a.Correo.Contains(SearchTerm) ||
                    a.Semestre.Contains(SearchTerm) ||
                    a.Telefono.Contains(SearchTerm)
                );
            }

            ListaAlumnos = await query
                .OrderBy(a => a.Nombre)
                .ToListAsync();
        }
    }
}