using System;
using System.Collections.Generic;
using System.Text;

namespace Parqueadero.Core.Entities
{
    public class TipoVehiculo
    {
        public int IdTipoVehiculo { get; set; }
        public string Descripcion { get; set; }
        public double Tarifa { get; set; }
    }
}
