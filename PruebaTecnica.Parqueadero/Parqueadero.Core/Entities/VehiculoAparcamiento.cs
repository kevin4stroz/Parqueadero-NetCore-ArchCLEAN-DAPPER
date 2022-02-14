using System;
using System.Collections.Generic;
using System.Text;

namespace Parqueadero.Core.Entities
{
    public class VehiculoAparcamiento
    {
        public int IdVehiculoAparcamiento { get; set; }
        public Vehiculo Vehiculo { get; set; }
        public Aparcamiento Aparcamiento { get; set; }
        public DateTime HoraLLegada { get; set; }
        public DateTime HoraSalida { get; set; }
        public double TiempoParqueo { get; set; }
        public double ValorPagar { get; set; }
        public Establecimiento Establecimiento { get; set; }
        public string NroFactura { get; set; }
    }
}
