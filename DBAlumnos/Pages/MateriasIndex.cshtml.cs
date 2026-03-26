using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DBAlumnos.Data;
using DBAlumnos.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBAlumnos.Pages
{
    public class MateriasIndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public MateriasIndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Materia> ListaMaterias { get; set; }

        public async Task OnGetAsync()
        {
            ListaMaterias = await _context.Materias
                .OrderBy(m => m.Nombre)
                .ToListAsync();
        }
    }
}