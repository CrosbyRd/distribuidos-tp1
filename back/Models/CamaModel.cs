namespace back.Models
{
    public class CamaModel
    {
        public int Id { get; set; }
        public bool EstaOcupado { get; set; }
        public int Orden { get; set; }
        public HospitalModel Hospital { get; set; }
    }
}