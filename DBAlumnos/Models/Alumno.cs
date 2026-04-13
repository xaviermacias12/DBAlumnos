using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DBAlumnos.Models
{
    public class Alumno
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La carrera es obligatoria")]
        public string Carrera { get; set; }

        [Required(ErrorMessage = "El semestre es obligatorio")]
        public string Semestre { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        public string Correo { get; set; }

        public string Telefono { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        public DateTime FechaNacimiento { get; set; }

        public string? FotoUrl { get; set; }

        public List<AlumnoMateria> AlumnoMaterias { get; set; }

        public Alumno()
        {
            AlumnoMaterias = new List<AlumnoMateria>();
        }
    }
}