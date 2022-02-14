using System;
using System.Collections.Generic;
using System.Text;

namespace Parqueadero.Core.DTO
{
    public class RegistroVehiculo
    {
        public string IdentificacionVehiculo { get; set; }
        public string Marca { get; set; }
        public string Color { get; set; }
        public int IdTipoVehiculo { get; set; }
    }
}
