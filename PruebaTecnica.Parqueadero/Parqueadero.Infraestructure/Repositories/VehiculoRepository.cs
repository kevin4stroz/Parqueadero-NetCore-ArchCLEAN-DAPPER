using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Parqueadero.Core.Entities;
using Parqueadero.Core.Interfaces;
using System.Data;
using Dapper;
using System.Linq;
using Parqueadero.Core.DTO;

namespace Parqueadero.Infraestructure.Repositories
{
    public class VehiculoRepository : IVehiculoRepository
    {
        private readonly IDbConnection _dbConnection;

        public VehiculoRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Vehiculo> Add(RegistroVehiculo regVeh)
        {
            string sqlQuery = @"INSERT INTO Parqueadero.Vehiculo VALUES
                                (@Placa,@Marca,@Color,@IdTipoVehiculo);

                                SELECT veh.*, tipoveh.* FROM [Parqueadero].[Vehiculo] as veh
                                INNER JOIN[Parqueadero].[TipoVehiculo] as tipoveh
                                    on veh.IdTipoVehiculo = tipoveh.IdTipoVehiculo
                                WHERE veh.IdVehiculo = SCOPE_IDENTITY()";

            var table = await _dbConnection.QueryAsync<Vehiculo, TipoVehiculo, Vehiculo>(
                sqlQuery,
                (vehiculo, tipoVehiculo) => {
                    vehiculo.TipoVehiculo = tipoVehiculo;
                    return vehiculo;
                },
                new { 
                    @Placa = regVeh.IdentificacionVehiculo, 
                    @Marca = regVeh.Marca,
                    @Color = regVeh.Color,
                    @IdTipoVehiculo = regVeh.IdTipoVehiculo },
                splitOn: "IdTipoVehiculo"
            );

            return table.FirstOrDefault();
        }

        public async Task<Vehiculo> GetByIdAsync(int Id)
        {
            string sqlQuery = "SELECT veh.*, tipoveh.* FROM [Parqueadero].[Vehiculo] as veh " +
                              "INNER JOIN[Parqueadero].[TipoVehiculo] as tipoveh on veh.IdTipoVehiculo = tipoveh.IdTipoVehiculo " +
                              "WHERE veh.IdVehiculo = @Id";
            var table = await _dbConnection.QueryAsync<Vehiculo, TipoVehiculo, Vehiculo>(
                sqlQuery,
                (vehiculo, tipoVehiculo) => {
                    vehiculo.TipoVehiculo = tipoVehiculo;
                    return vehiculo;
                },
                new { Id = Id },
                splitOn: "IdTipoVehiculo"
            );

            return table.FirstOrDefault();
        }

        public async Task<Vehiculo> GetByIdentification(string Identification)
        {
            string sqlQuery = "SELECT veh.*, tipoveh.* FROM [Parqueadero].[Vehiculo] as veh " +
                              "INNER JOIN[Parqueadero].[TipoVehiculo] as tipoveh on veh.IdTipoVehiculo = tipoveh.IdTipoVehiculo " +
                              "WHERE veh.IdentificacionVehiculo = @Id";
            var table = await _dbConnection.QueryAsync<Vehiculo, TipoVehiculo, Vehiculo>(
                sqlQuery,
                (vehiculo, tipoVehiculo) => {
                    vehiculo.TipoVehiculo = tipoVehiculo;
                    return vehiculo;
                },
                new { Id = Identification },
                splitOn: "IdTipoVehiculo"
            );

            return table.FirstOrDefault();
        }
    }
}
