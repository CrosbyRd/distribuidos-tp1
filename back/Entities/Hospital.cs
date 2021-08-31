using System.Collections.Generic;

namespace back.Entities
{
    public class Hospital
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<Cama> Camas { get; set; }
    }
}