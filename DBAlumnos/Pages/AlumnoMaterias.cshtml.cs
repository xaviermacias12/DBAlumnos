using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DBAlumnos.Data;
using DBAlumnos.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBAlumnos.Pages
{
    public class AlumnoMateriasModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AlumnoMateriasModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<AlumnoMateria> Lista { get; set; }

        public async Task OnGetAsync()
        {
            Lista = await _context.AlumnoMaterias
                .Include(am => am.Alumno)
                .Include(am => am.Materia)
                .ToListAsync();
        }
    }
}