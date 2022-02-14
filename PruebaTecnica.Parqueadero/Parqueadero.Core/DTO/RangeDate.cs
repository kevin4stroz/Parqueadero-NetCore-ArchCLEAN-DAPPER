using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Parqueadero.Core.DTO
{
    public class RangeDate
    {
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
    }
}
