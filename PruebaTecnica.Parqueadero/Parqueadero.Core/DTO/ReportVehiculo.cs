using System;
using System.Collections.Generic;
using System.Text;
using Parqueadero.Core.Entities;

namespace Parqueadero.Core.DTO
{
    public class ReportVehiculo
    {

        public ReportVehiculo(VehiculoAparcamiento vehAparca)
        {
            TipoVehiculo = vehAparca.Vehiculo.TipoVehiculo.Descripcion;
            Placa = vehAparca.Vehiculo.IdentificacionVehiculo;
            Estado = (vehAparca.HoraSalida ==default(DateTime)) ? 
                "NO LIQUIDADO" : "LIQUIDADO";
            ValorPagado = vehAparca.ValorPagar;
            TiempoParqueadero = (vehAparca.TiempoParqueo == 0) ? 
                (DateTime.Now - vehAparca.HoraLLegada).TotalHours : vehAparca.TiempoParqueo ;
            Aparcamiento = vehAparca.Aparcamiento.Nomeclatura;
            FechaFin = vehAparca.HoraSalida;
            FechaInicio = vehAparca.HoraLLegada;

        }

        public string Estado { get; set; }
        public string Placa { get; set; }
        public string TipoVehiculo { get; set; }
        public string Aparcamiento { get; set; }
        public double TiempoParqueadero { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public double ValorPagado { get; set; }
    }
}
