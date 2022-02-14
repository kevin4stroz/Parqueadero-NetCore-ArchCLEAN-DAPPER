using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Parqueadero.BussinesLogic.ParqueaderoBI;
using Parqueadero.Core.DTO;

namespace Parqueadero.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParqueaderoController : ControllerBase
    {
        private readonly ILogger<ParqueaderoController> _logger;
        private readonly IParqueaderoBI _parqueaderoBI;

        public ParqueaderoController(ILogger<ParqueaderoController> log, IParqueaderoBI parqueaderoBI)
        {
            _logger = log;
            _parqueaderoBI = parqueaderoBI;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RegistrarVehiculo(RegistroVehiculo regVehiculo)
        {
            // log
            _logger.LogInformation("[ENDPOINT] => api/Parqueadero/RegistrarVehiculo");
            _logger.LogInformation("[PAYLOAD] => regVehiculo : {0}", JsonSerializer.Serialize(regVehiculo));

            // validacion de modelo
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values);

            // validacion de campos nullos
            if (regVehiculo.IdentificacionVehiculo == null || regVehiculo.Marca == null ||
                regVehiculo.Color == null || regVehiculo.IdTipoVehiculo == 0)
                return BadRequest("Campos no pueden venir nulos");

            // validacion de campos vacios
            if (regVehiculo.IdentificacionVehiculo.Equals(string.Empty) || 
                regVehiculo.Marca.Equals(string.Empty) ||
                regVehiculo.Color.Equals(string.Empty) || regVehiculo.IdTipoVehiculo < 0)
                return BadRequest("Campos no pueden venir vacios");


            // ejecucion de repositorio atravez de unit of work para la funcion de registrar vehiculo
            var vehiculcoAparcamientoNuevo = await _parqueaderoBI.RegistrarVehiculo(regVehiculo);
            
            if(vehiculcoAparcamientoNuevo == null)
                return BadRequest("Error al registrar vehiculo, posiblemente ya este ingresado o falte info");

            return Ok(vehiculcoAparcamientoNuevo);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ListarVehiculos(RangeDate rangeDate) 
        {
            // log de peticion y payload de peticion
            _logger.LogInformation("[ENDPOINT] => POST api/Parqueadero/ListarVehiculos");
            _logger.LogInformation("[PAYLOAD] => rangeDate : {0}", JsonSerializer.Serialize(rangeDate));

            // validacion del modelstate
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values);

            // validar si las fechas llegan vacias
            if (rangeDate.fechaInicio == default(DateTime) || rangeDate.fechaFin == default(DateTime))
                return BadRequest("Debe definir fechaInicio y fechaFin");

            // validar si la fecha de inicio es menor que la fecha fin
            if (rangeDate.fechaInicio.CompareTo(rangeDate.fechaFin) >= 0)
                return BadRequest("Fecha de inicio debe ser menor que la fecha fin");

            // ejecucion de repositorio atravez de unit of work para la funcion de listas por fechas
            var listVehiculosRangeDate = await _parqueaderoBI.ListarVehiculos(rangeDate);

            return Ok(listVehiculosRangeDate);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> LiquidarVehiculo(LiquidarVehiculo liqVehiculo) 
        {
            // logs
            _logger.LogInformation("[ENDPOINT] => api/Parqueadero/LiquidarVehiculo");
            _logger.LogInformation("[PAYLOAD] => placa : {0}", liqVehiculo);

            // validar que modelo cumpla
            if (!ModelState.IsValid)
                return BadRequest("Debe suministrar placa");

            // validar si cambos vienen vacios
            if (liqVehiculo.Placa == null || liqVehiculo.Placa.Equals(string.Empty))
                return BadRequest("Placa es requerido");

            // ejecutar salida del vehiculo
            ReportVehiculo salidaVehiculo = await _parqueaderoBI.LiquidarVehiculo(liqVehiculo);

            if(salidaVehiculo==null)
                return BadRequest("Error liquidar vehiculo, no existe vehiculo o no existe establecimiento");

            return Ok(salidaVehiculo);
        }
    
        [HttpGet("[action]")]
        public IActionResult ManejadorErrores()
        {
            // generar exception
            throw new InvalidOperationException("Prueba de middleware exceptions");
        }

    }
}
