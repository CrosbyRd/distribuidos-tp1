using System;
using System.Threading.Tasks;
using back.Entities;

namespace back.Controllers
{
    public class LogController
    {
        private readonly UtiContext _context;

        public LogController(UtiContext context)
        {
            _context = context;
        }

        public async Task HacerLog(string camaObjetivo, string descripcion, int estado, string hospitalObjetivo,
            string nombreOperacion)
        {
            var newLog = new Log
            {
                CamaObjetivo = camaObjetivo,
                CreatedAt = DateTime.Now,
                Descripcion = descripcion,
                Estado = estado,
                HospitalObjetivo = hospitalObjetivo,
                NombreOperacion = nombreOperacion
            };

            await _context.Logs.AddAsync(newLog);

            await _context.SaveChangesAsync();
        }
    }
}