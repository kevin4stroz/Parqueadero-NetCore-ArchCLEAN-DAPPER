using System;
using System.Collections.Generic;
using System.Text;

namespace Parqueadero.Core.Entities
{
    public class Aparcamiento
    {
        public int IdAparcamiento { get; set; }
        public string Nomeclatura { get; set; }
        public bool Estado { get; set; }
    }
}
