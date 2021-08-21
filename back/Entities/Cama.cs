namespace back.Entities
{
    public class Cama
    {
        public int Id { get; set; }
        public bool EstaOcupado { get; set; }
        public int Orden { get; set; }
        
        public int? HospitalId { get; set; }
        public Hospital Hospital { get; set; }
    }
}