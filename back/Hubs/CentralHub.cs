using System.Threading.Tasks;
using back.Controllers;
using Microsoft.AspNetCore.SignalR;

namespace back.Hubs
{
    public class CentralHub : Hub
    {
        private readonly HospitalController _hospitalController; //variable que contiene a todo lo que tiene que ver con hospitales
        private readonly CamaController _camaController; //variable que contiene a todo lo que tiene que ver con camas

        public CentralHub(HospitalController hospitalController, CamaController camaController) //constructor comun y silvestre
        {
            _hospitalController = hospitalController;
            _camaController = camaController;
        }

        public async Task VerEstadoActual() //metodo que se llama desde el front end para invocarlo
        {
            //metodo para que envie a todos los que esten conectados al socket los estados de todos los hospitales
            await Clients.All.SendAsync("EstadoRecibido", _hospitalController.VerEstados());
            
        }
    }
}