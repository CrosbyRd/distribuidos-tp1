using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using back.Controllers;
using Microsoft.AspNetCore.SignalR;

namespace back.Hubs
{
    public class CentralHub : Hub
    {
        private readonly HospitalController _hospitalController;
        private readonly CamaController _camaController;

        public CentralHub(HospitalController hospitalController, CamaController camaController)
        {
            _hospitalController = hospitalController;
            _camaController = camaController;
        }

        public async Task VerEstadoActual()
        {
            var currentUserId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var resultList = await _hospitalController.VerEstados();

            await Clients.User(currentUserId).SendAsync("EstadoRecibido", resultList);
        }

        public async Task CrearHospital(string nombre)
        {
            var result = await _hospitalController.CrearHospital(nombre);
            await Clients.All.SendAsync("HospitalCreado", result);
        }

        public async Task EliminarHospital()
        {
            var result = await _hospitalController.EliminarHospital(CancellationToken.None);
            await Clients.All.SendAsync("HospitalEliminado", result);
        }

        public async Task CrearCama(int hospitalId)
        {
            var result = await _camaController.CrearCama(hospitalId);
            await Clients.All.SendAsync("CamaCreada", result);
        }

        public async Task EliminarCama(int camaId)
        {
            var result = await _camaController.EliminarCama(camaId);
            await Clients.All.SendAsync("CamaEliminada", result);
        }

        public async Task OcuparCama(int camaId)
        {
            var result = await _camaController.OcuparCama(camaId);
            await Clients.All.SendAsync("CamaOcupada", result);
        }

        public async Task DesocuparCama(int camaId)
        {
            var result = await _camaController.DesocuparCama(camaId);
            await Clients.All.SendAsync("CamaDesocupada", result);
        }
    }
}