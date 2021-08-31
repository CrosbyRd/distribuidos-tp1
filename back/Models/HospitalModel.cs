using System.Collections.Generic;

namespace back.Models
{
    public class HospitalModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<CamaModel> Camas { get; set; }
    }
}