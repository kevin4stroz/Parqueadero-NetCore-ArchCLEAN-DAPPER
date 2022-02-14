using System;
using System.Collections.Generic;
using System.Text;
using Parqueadero.Core.Interfaces;
using Parqueadero.Core.Entities;
using System.Threading.Tasks;
using System.Data;
using Dapper;

namespace Parqueadero.Infraestructure.Repositories
{
    public class AparcamientoRepository : IAparcamientoRepository
    {
        private readonly IDbConnection _dbConnection;

        public AparcamientoRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<bool> ExistAparcaUseByIdentifi(string Id)
        {
            string sqlQuery = @"SELECT top 1 aparca.* FROM Parqueadero.Aparcamiento aparca
                                INNER JOIN Parqueadero.VehiculoAparcamiento vehAparca 
	                                ON aparca.IdAparcamiento = vehAparca.IdAparcamiento
                                INNER JOIN Parqueadero.Vehiculo veh
	                                ON vehAparca.IdVehiculo = veh.IdVehiculo
                                WHERE 
	                                veh.IdentificacionVehiculo = @Id AND aparca.Estado = 1
                                ORDER BY vehAparca.HoraLLegada DESC";

            var tableReturn = await _dbConnection.QuerySingleOrDefaultAsync<Aparcamiento>(
                sqlQuery, new { Id = Id }
            );

            return tableReturn != null;
        }

        public async Task<Aparcamiento> GetByIdAsync(int Id)
        {
            const string sqlQuery = "SELECT * FROM [Parqueadero].[Aparcamiento] WHERE IdAparcamiento=@Id";
            return await _dbConnection.QuerySingleOrDefaultAsync<Aparcamiento>(sqlQuery, new { Id = Id });
        }

        public async Task<Aparcamiento> GetFreeAparca()
        {
            string sqlQuery = @"SELECT top 1 * FROM Parqueadero.Aparcamiento aparca WHERE aparca.Estado = 0";

            var tableReturn = await _dbConnection.QuerySingleOrDefaultAsync<Aparcamiento>(
                sqlQuery
            );

            return tableReturn;
        }
    }
}
