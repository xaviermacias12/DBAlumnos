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
    public class CrearAlumnoModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CrearAlumnoModel(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public Alumno Alumno { get; set; }

        [BindProperty]
        public IFormFile? Foto { get; set; }  // ✅ Hacer opcional

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Validar el modelo manualmente
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Verificar si ya existe un alumno con el mismo correo
            var existe = await _context.Alumnos
                .AnyAsync(a => a.Correo == Alumno.Correo);

            if (existe)
            {
                ModelState.AddModelError("Alumno.Correo", "Ya existe un alumno con este correo");
                return Page();
            }

            // Guardar la foto SOLO si se subió una
            if (Foto != null && Foto.Length > 0)
            {
                // Validar extensión
                var extension = Path.GetExtension(Foto.FileName).ToLower();
                if (extension != ".jpg" && extension != ".png" && extension != ".jpeg" && extension != ".gif")
                {
                    ModelState.AddModelError("Foto", "Solo se permiten archivos de imagen (JPG, PNG, GIF)");
                    return Page();
                }

                // Validar tamaño (máximo 2MB)
                if (Foto.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("Foto", "La imagen no puede superar los 2MB");
                    return Page();
                }

                // Crear nombre único para la foto
                var nombreArchivo = $"{Guid.NewGuid()}{extension}";
                var rutaCarpeta = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "alumnos");

                // Crear carpeta si no existe
                if (!Directory.Exists(rutaCarpeta))
                {
                    Directory.CreateDirectory(rutaCarpeta);
                }

                var rutaArchivo = Path.Combine(rutaCarpeta, nombreArchivo);

                // Guardar archivo
                using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                {
                    await Foto.CopyToAsync(stream);
                }

                Alumno.FotoUrl = $"/uploads/alumnos/{nombreArchivo}";
            }
            else
            {
                // Si no hay foto, asignar null o vacío
                Alumno.FotoUrl = null;
            }

            _context.Alumnos.Add(Alumno);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Alumno creado con éxito";
            return RedirectToPage("/Index");
        }
    }
}