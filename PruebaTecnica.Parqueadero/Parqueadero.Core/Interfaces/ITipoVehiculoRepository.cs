using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Parqueadero.Core.Entities;

namespace Parqueadero.Core.Interfaces
{
    public interface ITipoVehiculoRepository
    {
        Task<TipoVehiculo> GetByIdAsync(int Id);
    }
}
