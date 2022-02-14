using System;
using System.Collections.Generic;
using System.Text;

namespace Parqueadero.Core.Interfaces
{
    public interface IUnitOfWork
    {
        ITipoVehiculoRepository TipoVehiculo { get; }
        IVehiculoRepository Vehiculo { get; }
        IAparcamientoRepository Aparcamiento { get; }
        IEstablecimientoRepository Establecimiento { get; }
        IVehiculoAparcamientoRepository VehiculoAparcamiento { get; }
    }
}
