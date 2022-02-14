using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Parqueadero.Core.Entities;
using Parqueadero.Core.Interfaces;
using System.Data;
using Dapper;

namespace Parqueadero.Infraestructure.Repositories
{
    public class TipoVehiculoRepository : ITipoVehiculoRepository
    {
        private readonly IDbConnection _dbConnection;

        public TipoVehiculoRepository(IDbConnection dbConnection) 
        {
            _dbConnection = dbConnection;
        }

        public async Task<TipoVehiculo> GetByIdAsync(int Id)
        {
            const string sqlQuery= "SELECT * FROM [Parqueadero].[TipoVehiculo] WHERE IdTipoVehiculo=@Id";
            return await _dbConnection.QuerySingleOrDefaultAsync<TipoVehiculo>(sqlQuery, new { Id = Id});
        }
    }
}
