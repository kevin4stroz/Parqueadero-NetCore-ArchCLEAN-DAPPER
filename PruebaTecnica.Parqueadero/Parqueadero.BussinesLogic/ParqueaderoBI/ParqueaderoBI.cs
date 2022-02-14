using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Parqueadero.Core.Interfaces;
using Parqueadero.Core.DTO;
using Parqueadero.Core.Entities;

namespace Parqueadero.BussinesLogic.ParqueaderoBI
{
    public class ParqueaderoBI : IParqueaderoBI
    {
        private readonly ILogger<ParqueaderoBI> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public ParqueaderoBI(ILogger<ParqueaderoBI> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<ReportVehiculo> LiquidarVehiculo(LiquidarVehiculo liqVehiculo)
        {
            _logger.LogInformation("[BUSSINES_LOGIC] => ParqueaderoBI : Ejecucion liquidar vehiculo");


            // verificar que existe un VehiculoAparcamiento con fechaFin null y asociado a el vehiculo
            VehiculoAparcamiento vehAparcaCurrent = await _unitOfWork.VehiculoAparcamiento.GetByIdentificacionVehiculo(
                liqVehiculo.Placa
            );

            if (vehAparcaCurrent == null)
                return null;

            // calcular salida
            vehAparcaCurrent.HoraSalida = DateTime.Now;
            vehAparcaCurrent.TiempoParqueo = (vehAparcaCurrent.HoraSalida - vehAparcaCurrent.HoraLLegada).TotalHours;
            vehAparcaCurrent.ValorPagar = (vehAparcaCurrent.TiempoParqueo * vehAparcaCurrent.Vehiculo.TipoVehiculo.Tarifa);

            // verificar si existe establecimiento
            if (liqVehiculo.IdEstablecimiento != 0)
            {
                if (liqVehiculo.IdEstablecimiento <= 0 || liqVehiculo.NumeroFactura == null ||
                    liqVehiculo.NumeroFactura.Equals(string.Empty))
                    return null;

                Establecimiento establecimientoTemp = await _unitOfWork.Establecimiento.GetByIdAsync(liqVehiculo.IdEstablecimiento);
                if (establecimientoTemp == null)
                    return null;

                vehAparcaCurrent.Establecimiento = establecimientoTemp;
                vehAparcaCurrent.NroFactura = liqVehiculo.NumeroFactura;

                vehAparcaCurrent.ValorPagar = vehAparcaCurrent.ValorPagar - (vehAparcaCurrent.ValorPagar * vehAparcaCurrent.Establecimiento.TarifaDescuento) / 100;
            }

            // liquidar vehiculo
            bool statusOperation = await _unitOfWork.VehiculoAparcamiento.Update(
                vehAparcaCurrent.IdVehiculoAparcamiento, vehAparcaCurrent.HoraSalida, vehAparcaCurrent.TiempoParqueo,
                vehAparcaCurrent.ValorPagar, vehAparcaCurrent.Establecimiento.IdEstablecimiento, vehAparcaCurrent.NroFactura,
                vehAparcaCurrent.Aparcamiento.IdAparcamiento
            );


            return statusOperation ? new ReportVehiculo(vehAparcaCurrent) : null;
        }

        public async Task<List<ReportVehiculo>> ListarVehiculos(RangeDate rangDate)
        {
            // log 
            _logger.LogInformation("[BUSSINES_LOGIC] => ParqueaderoBI : Ejecucion listar vehiculos");

            // ejecucion de unit of work para obtener vehiculo en rango de fecha
            var listVehiculosRangeDate = await _unitOfWork.VehiculoAparcamiento.GetByRangeDate(rangDate);

            // mapeo al DTO de salida
            List<ReportVehiculo> listReturned = listVehiculosRangeDate.Select( 
                x => new ReportVehiculo(x)
            ).ToList();

            return listReturned;
        }

        public async Task<ReportVehiculo> RegistrarVehiculo(RegistroVehiculo regVehiculo)
        {
            // log
            _logger.LogInformation("[BUSSINES_LOGIC] => ParqueaderoBI : Ejecucion Registrar vehiculo");

            // verificar si existe vehiculo por identificacion
            Vehiculo vehCurrent = await _unitOfWork.Vehiculo.GetByIdentification(
                regVehiculo.IdentificacionVehiculo
            );

            if(vehCurrent != null)
            {
                // si existe se debe validar que no tenga un  abierto
                if (await _unitOfWork.Aparcamiento.ExistAparcaUseByIdentifi(regVehiculo.IdentificacionVehiculo))
                    return null;
            }
            else
            {
                // si no existe se debe validar el tipo de vehiculo
                if (await _unitOfWork.TipoVehiculo.GetByIdAsync(regVehiculo.IdTipoVehiculo) == null)
                    return null;

                // si no existe insertarlo y obtener el objeto
                vehCurrent = await _unitOfWork.Vehiculo.Add(regVehiculo);
                if (vehCurrent == null)
                    return null;
            }

            // obtener un aparcamiento disponible
            Aparcamiento aparcaTemp = await _unitOfWork.Aparcamiento.GetFreeAparca();

            // insertar vehApacarmiento
            return new ReportVehiculo(
                await _unitOfWork.VehiculoAparcamiento.Add(vehCurrent.IdVehiculo, aparcaTemp.IdAparcamiento)
            );
        }
    }
}
