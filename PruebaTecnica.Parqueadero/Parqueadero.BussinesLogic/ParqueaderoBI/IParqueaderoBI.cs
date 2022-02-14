using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Parqueadero.Core.Entities;
using Parqueadero.Core.DTO;

namespace Parqueadero.BussinesLogic.ParqueaderoBI
{
    public interface IParqueaderoBI
    {
        Task<ReportVehiculo> RegistrarVehiculo(RegistroVehiculo regVehiculo);
        Task<List<ReportVehiculo>> ListarVehiculos(RangeDate rangDate);
        Task<ReportVehiculo>LiquidarVehiculo(LiquidarVehiculo lisVehiculo);
    }
}
