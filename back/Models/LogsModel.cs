using System;

namespace back.Models
{
    public class LogsModel
    {
        public int Id { get; set; }
        public string NombreOperacion { get; set; }
        public int Estado { get; set; }
        public string Descripcion { get; set; }
        public string HospitalObjetivo { get; set; }
        public string CamaObjetivo { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}