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
    public class EstablecimientoRepository : IEstablecimientoRepository
    {
        private readonly IDbConnection _dbConnection;
        public EstablecimientoRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Establecimiento> GetByIdAsync(int Id)
        {
            const string sqlQuery = "SELECT * FROM [Parqueadero].[Establecimiento] WHERE IdEstablecimiento=@Id";
            return await _dbConnection.QuerySingleOrDefaultAsync<Establecimiento>(sqlQuery, new { Id = Id });
        }
    }
}
