using System;
using System.Collections.Generic;
using System.Text;
using Parqueadero.Core.Entities;
using Parqueadero.Core.Interfaces;
using System.Data;
using Dapper;
using System.Threading.Tasks;
using Parqueadero.Core.DTO;

namespace Parqueadero.Infraestructure.Repositories
{
    public class VehiculoAparcamientoRepository : IVehiculoAparcamientoRepository
    {
        private readonly IDbConnection _dbConnection;

        public VehiculoAparcamientoRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<VehiculoAparcamiento> Add(int IdVehiculo, int IdAparcamiento)
        {
            string sqlQuery = @"UPDATE Parqueadero.Aparcamiento SET Estado = 1
                                WHERE IdAparcamiento = @IdAparcamiento;

                                INSERT INTO [Parqueadero].[VehiculoAparcamiento] VALUES 
                                (@IdVehiculo, @IdAparcamiento, GETDATE(), NULL, NULL, NULL, NULL, NULL);

                                SELECT 
                                    vehaparca.IdVehiculoAparcamiento S1, vehaparca.*, 
                                    veh.IdVehiculo S2, veh.*, 
                                    tipoveh.IdTipoVehiculo S3, tipoveh.*, 
                                    aparca.IdAparcamiento S4, aparca.*,
                                    estable.IdEstablecimiento S5, estable.*
                                FROM [Parqueadero].[VehiculoAparcamiento] as vehaparca 
                                INNER JOIN[Parqueadero].[Vehiculo] as veh 
                                    on vehaparca.IdVehiculo = veh.IdVehiculo 
                                INNER JOIN[Parqueadero].[TipoVehiculo] as tipoveh 
                                    on veh.IdTipoVehiculo = tipoveh.IdTipoVehiculo
                                INNER JOIN[Parqueadero].[Aparcamiento] as aparca 
                                    on vehaparca.IdAparcamiento = aparca.IdAparcamiento 
                                LEFT JOIN[Parqueadero].[Establecimiento] as estable 
                                    on vehaparca.IdEstablecimiento = estable.IdEstablecimiento
                                WHERE vehaparca.IdVehiculoAparcamiento = SCOPE_IDENTITY();";

            var tableQuery = await _dbConnection.QueryAsync<VehiculoAparcamiento, Vehiculo,
                                              TipoVehiculo, Aparcamiento, Establecimiento,
                                              VehiculoAparcamiento>(
                                sqlQuery,
                                (vehaparca, veh, tipoveh, aparca, estable) => {
                                    VehiculoAparcamiento vehAparca = vehaparca;
                                    Vehiculo vehTemp = veh;
                                    vehTemp.TipoVehiculo = tipoveh;
                                    Aparcamiento aparcaT = aparca;
                                    Establecimiento estableT = estable;

                                    vehAparca.Vehiculo = vehTemp;
                                    vehAparca.Aparcamiento = aparcaT;
                                    vehAparca.Establecimiento = estableT;

                                    return vehAparca;
                                },
                                new { 
                                    IdVehiculo = IdVehiculo,
                                    IdAparcamiento = IdAparcamiento
                                },
                                splitOn: "S1,S2,S3,S4,S5"
            );

            List<VehiculoAparcamiento> returnVal = null;
            try
            {
                returnVal = tableQuery.AsList<VehiculoAparcamiento>();
            }
            catch
            {
                returnVal = null;
            }

            return returnVal != null && returnVal.Count == 1 ? returnVal[0] : null;
        }

        public async Task<VehiculoAparcamiento> GetByIdAsync(int Id)
        {
            string sqlQuery = @"SELECT 
                                    vehaparca.IdVehiculoAparcamiento S1, vehaparca.*, 
                                    veh.IdVehiculo S2, veh.*, 
                                    tipoveh.IdTipoVehiculo S3, tipoveh.*, 
                                    aparca.IdAparcamiento S4, aparca.*,
                                    estable.IdEstablecimiento S5, estable.*
                              FROM [Parqueadero].[VehiculoAparcamiento] as vehaparca 
                              INNER JOIN[Parqueadero].[Vehiculo] as veh 
                                    on vehaparca.IdVehiculo = veh.IdVehiculo 
                              INNER JOIN[Parqueadero].[TipoVehiculo] as tipoveh 
                                    on veh.IdTipoVehiculo = tipoveh.IdTipoVehiculo
                              INNER JOIN[Parqueadero].[Aparcamiento] as aparca 
                                    on vehaparca.IdAparcamiento = aparca.IdAparcamiento 
                              LEFT JOIN[Parqueadero].[Establecimiento] as estable 
                                    on vehaparca.IdEstablecimiento = estable.IdEstablecimiento
                              WHERE vehaparca.IdVehiculoAparcamiento = @Id";

            var tableQuery = await _dbConnection.QueryAsync<VehiculoAparcamiento, Vehiculo, 
                                              TipoVehiculo, Aparcamiento, Establecimiento, 
                                              VehiculoAparcamiento>(
                                sqlQuery,
                                (vehaparca, veh, tipoveh, aparca, estable) => {
                                    VehiculoAparcamiento vehAparca = vehaparca;
                                    Vehiculo vehTemp = veh;
                                    vehTemp.TipoVehiculo = tipoveh;
                                    Aparcamiento aparcaT = aparca;
                                    Establecimiento estableT = estable;

                                    vehAparca.Vehiculo = vehTemp;
                                    vehAparca.Aparcamiento = aparcaT;
                                    vehAparca.Establecimiento = estableT;

                                    return vehAparca;
                                },
                                new { Id = Id },
                                splitOn : "S1,S2,S3,S4,S5"
            );

            List<VehiculoAparcamiento> returnVal = null;
            try
            {
                returnVal = tableQuery.AsList<VehiculoAparcamiento>();
            }
            catch
            {
                returnVal = null;
            }

            return returnVal != null && returnVal.Count == 1 ? returnVal[0] : null;
        }

        public async Task<VehiculoAparcamiento> GetByIdentificacionVehiculo(string placa)
        {
            string sqlQuery = @"SELECT top 1
                                    vehaparca.IdVehiculoAparcamiento S1, vehaparca.*, 
                                    veh.IdVehiculo S2, veh.*, 
                                    tipoveh.IdTipoVehiculo S3, tipoveh.*, 
                                    aparca.IdAparcamiento S4, aparca.*,
                                    estable.IdEstablecimiento S5, estable.*
                              FROM [Parqueadero].[VehiculoAparcamiento] as vehaparca 
                              INNER JOIN [Parqueadero].[Vehiculo] as veh 
                                    on vehaparca.IdVehiculo = veh.IdVehiculo 
                              INNER JOIN [Parqueadero].[TipoVehiculo] as tipoveh 
                                    on veh.IdTipoVehiculo = tipoveh.IdTipoVehiculo
                              INNER JOIN [Parqueadero].[Aparcamiento] as aparca 
                                    on vehaparca.IdAparcamiento = aparca.IdAparcamiento 
                              LEFT JOIN [Parqueadero].[Establecimiento] as estable 
                                    on vehaparca.IdEstablecimiento = estable.IdEstablecimiento
                              WHERE vehaparca.HoraSalida is null AND veh.IdentificacionVehiculo = @Placa";

            var tableQuery = await _dbConnection.QueryAsync<VehiculoAparcamiento, Vehiculo,
                                              TipoVehiculo, Aparcamiento, Establecimiento,
                                              VehiculoAparcamiento>(
                                sqlQuery,
                                (vehaparca, veh, tipoveh, aparca, estable) => {
                                    VehiculoAparcamiento vehAparca = vehaparca;
                                    Vehiculo vehTemp = veh;
                                    vehTemp.TipoVehiculo = tipoveh;
                                    Aparcamiento aparcaT = aparca;
                                    Establecimiento estableT = estable;

                                    vehAparca.Vehiculo = vehTemp;
                                    vehAparca.Aparcamiento = aparcaT;
                                    vehAparca.Establecimiento = estableT;

                                    return vehAparca;
                                },
                                new { Placa = placa },
                                splitOn: "S1,S2,S3,S4,S5"
            );

            List<VehiculoAparcamiento> returnVal = null;
            try
            {
                returnVal = tableQuery.AsList<VehiculoAparcamiento>();
            }
            catch
            {
                returnVal = null;
            }

            return returnVal != null && returnVal.Count == 1 ? returnVal[0] : null;
        }

        public async Task<IEnumerable<VehiculoAparcamiento>> GetByRangeDate(RangeDate rangeDate)
        {
            string sqlQuery = @"SELECT 
                                    vehaparca.IdVehiculoAparcamiento S1, vehaparca.*, 
                                    veh.IdVehiculo S2, veh.*, 
                                    tipoveh.IdTipoVehiculo S3, tipoveh.*, 
                                    aparca.IdAparcamiento S4, aparca.*,
                                    estable.IdEstablecimiento S5, estable.*
                              FROM [Parqueadero].[VehiculoAparcamiento] as vehaparca 
                              INNER JOIN[Parqueadero].[Vehiculo] as veh 
                                    on vehaparca.IdVehiculo = veh.IdVehiculo 
                              INNER JOIN[Parqueadero].[TipoVehiculo] as tipoveh 
                                    on veh.IdTipoVehiculo = tipoveh.IdTipoVehiculo
                              INNER JOIN[Parqueadero].[Aparcamiento] as aparca 
                                    on vehaparca.IdAparcamiento = aparca.IdAparcamiento 
                              LEFT JOIN[Parqueadero].[Establecimiento] as estable 
                                    on vehaparca.IdEstablecimiento = estable.IdEstablecimiento
                              WHERE vehaparca.HoraLLegada BETWEEN @fechaInicio and @fechaFin";

            var tableQuery = await _dbConnection.QueryAsync<VehiculoAparcamiento, Vehiculo,
                                              TipoVehiculo, Aparcamiento, Establecimiento,
                                              VehiculoAparcamiento>(
                                sqlQuery,
                                (vehaparca, veh, tipoveh, aparca, estable) => {
                                    VehiculoAparcamiento vehAparca = vehaparca;
                                    Vehiculo vehTemp = veh;
                                    vehTemp.TipoVehiculo = tipoveh;
                                    Aparcamiento aparcaT = aparca;
                                    Establecimiento estableT = estable;

                                    vehAparca.Vehiculo = vehTemp;
                                    vehAparca.Aparcamiento = aparcaT;
                                    vehAparca.Establecimiento = estableT;

                                    return vehAparca;
                                },
                                new { 
                                    fechaInicio = rangeDate.fechaInicio,
                                    fechaFin = rangeDate.fechaFin
                                },
                                splitOn: "S1,S2,S3,S4,S5"
            );


            return tableQuery;
        }

        public async Task<bool> Update(
            int idVehAparca, DateTime fechaSalida, double tiempoParqueo, double valorPagar, 
            int IdEstable, string nFactura, int IdAparca
        )
        {

            var parameters = new
            {
                IdAparcamiento = IdAparca,
                HoraSalida = fechaSalida,
                TiempoParqueo = tiempoParqueo,
                ValorPagar = valorPagar,
                IdEstablecimiento = IdEstable,
                NroFactura = nFactura,
                IdVehiculoAparcamiento = idVehAparca
            };

            string sqlQuery = @"UPDATE Parqueadero.Aparcamiento 
                                SET 
	                                Estado = 0 
                                WHERE 
	                                IdAparcamiento = @IdAparcamiento;

                                UPDATE Parqueadero.VehiculoAparcamiento
                                SET 
	                                HoraSalida=@HoraSalida, TiempoParqueo=@TiempoParqueo, ValorPagar=@ValorPagar,
	                                IdEstablecimiento=@IdEstablecimiento, NroFactura=@NroFactura
                                WHERE 
	                                IdVehiculoAparcamiento=@IdVehiculoAparcamiento;";

            if (IdEstable == 0)
            {
                sqlQuery = sqlQuery.Replace("@IdEstablecimiento", "NULL");
                sqlQuery = sqlQuery.Replace("@NroFactura", "NULL");
            }

            try
            {
                await _dbConnection.QueryAsync(sqlQuery, parameters);
            }
            catch
            {
                return false;
            }           

            return true;
        }
    }
}
