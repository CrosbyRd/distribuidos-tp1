using System;
using System.Linq;
using System.Threading.Tasks;
using back.Entities;
using Microsoft.EntityFrameworkCore;

namespace back.Controllers
{
    public class CamaController
    {
        private readonly UtiContext _utiContext;

        public CamaController(UtiContext utiContext)
        {
            _utiContext = utiContext;
        }

        public async Task<int> CrearCama(int hospitalId)
        {
            try
            {
                var totalCamas = await _utiContext.Camas.CountAsync();
            
                var newCama = new Cama
                {
                    EstaOcupado = false,
                    HospitalId = hospitalId,
                    Orden = totalCamas + 1
                };

                await _utiContext.Camas.AddAsync(newCama);
            
                await _utiContext.SaveChangesAsync();
            
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
        }

        public async Task<int> EliminarCama(int camaId)
        {
            try
            {
                var cama = await _utiContext.Camas.FirstOrDefaultAsync(c => c.Id == camaId);
                
                _utiContext.Remove(cama);
                
                await _utiContext.SaveChangesAsync();
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
        }


        public async Task<int> OcuparCama(int camaId)
        {
            try
            {
                var cama = await _utiContext.Camas.FirstOrDefaultAsync(c => c.Id == camaId);

                cama.EstaOcupado = true;

                _utiContext.Camas.Update(cama);
                
                await _utiContext.SaveChangesAsync();
                
                return 0;
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
        }
        
        public async Task<int> DesocuparCama(int camaId)
        {
            try
            {
                var cama = await _utiContext.Camas.FirstOrDefaultAsync(c => c.Id == camaId);

                cama.EstaOcupado = false;

                _utiContext.Camas.Update(cama);
                
                await _utiContext.SaveChangesAsync();
                
                return 0;
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
        }
    }
}