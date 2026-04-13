using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DBAlumnos.Data;
using DBAlumnos.Models;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace DBAlumnos.Pages
{
    public class EditarAlumnoModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EditarAlumnoModel(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public Alumno Alumno { get; set; }

        [BindProperty]
        public IFormFile NuevaFoto { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Alumno = await _context.Alumnos.FindAsync(id);

            if (Alumno == null)
            {
                TempData["ErrorMessage"] = "Alumno no encontrado";
                return RedirectToPage("/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Verificar si ya existe otro alumno con el mismo correo
            var existe = await _context.Alumnos
                .AnyAsync(a => a.Correo == Alumno.Correo && a.Id != Alumno.Id);

            if (existe)
            {
                ModelState.AddModelError("Alumno.Correo", "Ya existe otro alumno con este correo");
                return Page();
            }

            // Obtener el alumno original para la foto
            var alumnoOriginal = await _context.Alumnos.AsNoTracking().FirstOrDefaultAsync(a => a.Id == Alumno.Id);

            // Manejar la nueva foto
            if (NuevaFoto != null && NuevaFoto.Length > 0)
            {
                // Validar extensión
                var extension = Path.GetExtension(NuevaFoto.FileName).ToLower();
                if (extension != ".jpg" && extension != ".png" && extension != ".jpeg" && extension != ".gif")
                {
                    ModelState.AddModelError("NuevaFoto", "Solo se permiten archivos de imagen (JPG, PNG, GIF)");
                    return Page();
                }

                // Validar tamaño
                if (NuevaFoto.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("NuevaFoto", "La imagen no puede superar los 2MB");
                    return Page();
                }

                // Eliminar foto anterior si existe
                if (!string.IsNullOrEmpty(alumnoOriginal.FotoUrl))
                {
                    var rutaAnterior = Path.Combine(_webHostEnvironment.WebRootPath,
                        alumnoOriginal.FotoUrl.TrimStart('/'));
                    if (System.IO.File.Exists(rutaAnterior))
                    {
                        System.IO.File.Delete(rutaAnterior);
                    }
                }

                // Guardar nueva foto
                var nombreArchivo = $"{Guid.NewGuid()}{extension}";
                var rutaCarpeta = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "alumnos");

                if (!Directory.Exists(rutaCarpeta))
                {
                    Directory.CreateDirectory(rutaCarpeta);
                }

                var rutaArchivo = Path.Combine(rutaCarpeta, nombreArchivo);

                using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                {
                    await NuevaFoto.CopyToAsync(stream);
                }

                Alumno.FotoUrl = $"/uploads/alumnos/{nombreArchivo}";
            }
            else
            {
                // Mantener la foto existente
                Alumno.FotoUrl = alumnoOriginal.FotoUrl;
            }

            _context.Attach(Alumno).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Alumno actualizado con éxito";
                return RedirectToPage("/Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await AlumnoExists(Alumno.Id))
                {
                    return NotFound();
                }
                throw;
            }
        }

        private async Task<bool> AlumnoExists(int id)
        {
            return await _context.Alumnos.AnyAsync(a => a.Id == id);
        }
    }
}