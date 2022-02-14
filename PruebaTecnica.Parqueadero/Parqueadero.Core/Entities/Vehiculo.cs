using System;
using System.Collections.Generic;
using System.Text;

namespace Parqueadero.Core.Entities
{
    public class Vehiculo
    {
        public int IdVehiculo { get; set; }
        public string IdentificacionVehiculo { get; set; }
        public string Marca { get; set; }
        public string Color { get; set; }
        public int IdTipoVehiculo { get; set; }
        public TipoVehiculo TipoVehiculo { get; set; }
    }
}
