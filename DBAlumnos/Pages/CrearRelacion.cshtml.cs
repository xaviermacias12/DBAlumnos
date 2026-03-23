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
    public class CrearRelacionModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CrearRelacionModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AlumnoMateria AlumnoMateria { get; set; }

        public List<Alumno> Alumnos { get; set; }
        public List<Materia> Materias { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Cargar listas de alumnos y materias
            Alumnos = await _context.Alumnos.OrderBy(a => a.Nombre).ToListAsync();
            Materias = await _context.Materias.OrderBy(m => m.Nombre).ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Validar que los campos no estén vacíos
            if (AlumnoMateria.AlumnoId == 0 || AlumnoMateria.MateriaId == 0)
            {
                ModelState.AddModelError(string.Empty, "Debe seleccionar un alumno y una materia.");
                await CargarListas();
                return Page();
            }

            // Verificar si la relación ya existe
            var existe = await _context.AlumnoMaterias
                .AnyAsync(am => am.AlumnoId == AlumnoMateria.AlumnoId
                             && am.MateriaId == AlumnoMateria.MateriaId);

            if (existe)
            {
                ModelState.AddModelError(string.Empty, "Esta relación ya existe. El alumno ya está inscrito en esta materia.");
                await CargarListas();
                return Page();
            }

            // Agregar la relación
            _context.AlumnoMaterias.Add(AlumnoMateria);
            await _context.SaveChangesAsync();

            return RedirectToPage("/AlumnoMaterias");
        }

        private async Task CargarListas()
        {
            Alumnos = await _context.Alumnos.OrderBy(a => a.Nombre).ToListAsync();
            Materias = await _context.Materias.OrderBy(m => m.Nombre).ToListAsync();
        }
    }
}