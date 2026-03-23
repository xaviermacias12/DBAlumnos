namespace DBAlumnos.Models
{
    public class AlumnoMateria
    {
        public int AlumnoId { get; set; }
        public Alumno Alumno { get; set; }
        public int MateriaId { get; set; }
        public Materia Materia { get; set; }
    }
}
