using System;
using System.Collections.Generic;
using System.Text;

namespace Parqueadero.Core.Entities
{
    public class Establecimiento
    {
        public int IdEstablecimiento { get; set; }
        public string NitCedula { get; set; }
        public string NombreEstablecimiento { get; set; }
        public float TarifaDescuento { get; set; }
    }
}
