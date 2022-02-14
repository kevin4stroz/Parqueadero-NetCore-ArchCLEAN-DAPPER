using System;
using System.Collections.Generic;
using System.Text;

namespace Parqueadero.Core.DTO
{
    public class LiquidarVehiculo
    {
        public string Placa { get; set; }
        public int IdEstablecimiento { get; set; }
        public string NumeroFactura { get; set; }
    }
}
