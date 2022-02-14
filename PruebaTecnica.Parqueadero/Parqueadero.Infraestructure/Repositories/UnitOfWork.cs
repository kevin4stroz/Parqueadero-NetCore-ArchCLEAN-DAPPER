using System;
using System.Collections.Generic;
using System.Text;
using Parqueadero.Core.Interfaces;
using Parqueadero.Core.Entities;

namespace Parqueadero.Infraestructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(
            ITipoVehiculoRepository tipoVehiculo,
            IVehiculoRepository vehiculo,
            IAparcamientoRepository aparcamiento,
            IEstablecimientoRepository establecimiento,
            IVehiculoAparcamientoRepository vehiculoAparcamiento            
        )
        {
            TipoVehiculo = tipoVehiculo;
            Vehiculo = vehiculo;
            Aparcamiento = aparcamiento;
            Establecimiento = establecimiento;
            VehiculoAparcamiento = vehiculoAparcamiento;
        }

        public ITipoVehiculoRepository TipoVehiculo { get; }

        public IVehiculoRepository Vehiculo { get; }

        public IAparcamientoRepository Aparcamiento { get; }

        public IEstablecimientoRepository Establecimiento { get; }

        public IVehiculoAparcamientoRepository VehiculoAparcamiento { get; }
    }
}
