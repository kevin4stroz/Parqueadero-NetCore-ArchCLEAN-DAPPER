using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Parqueadero.Core.Entities;
using Parqueadero.Core.DTO;

namespace Parqueadero.Core.Interfaces
{
    public interface IVehiculoRepository
    {
        Task<Vehiculo> GetByIdAsync(int Id);
        Task<Vehiculo> GetByIdentification(string Identification);
        Task<Vehiculo> Add(RegistroVehiculo regVeh);

    }
}
