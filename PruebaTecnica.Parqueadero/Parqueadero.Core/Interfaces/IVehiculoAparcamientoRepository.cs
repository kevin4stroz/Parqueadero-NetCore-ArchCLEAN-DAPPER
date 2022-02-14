using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Parqueadero.Core.Entities;
using Parqueadero.Core.DTO;

namespace Parqueadero.Core.Interfaces
{
    public interface IVehiculoAparcamientoRepository
    {
        Task<VehiculoAparcamiento> GetByIdAsync(int Id);
        Task<IEnumerable<VehiculoAparcamiento>> GetByRangeDate(RangeDate rangeDate);
        Task<VehiculoAparcamiento> Add(int IdVehiculo, int IdAparcamiento);
        Task<VehiculoAparcamiento> GetByIdentificacionVehiculo(string placa);
        Task<bool> Update(
            int idVehAparca, DateTime fechaSalida, double tiempoParqueo, 
            double valorPagar, int IdEstable, string nFactura, int IdAparca
        );
    }
}
