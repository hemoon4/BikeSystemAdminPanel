using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading.Tasks;
using BikeSystemAdminPanel.Models;
using Dapper;

namespace BikeSystemAdminPanel.Database
{
    public class SqliteStationRepository : IStationRepository
    {
        private readonly string _connectionString;

        public SqliteStationRepository(string dbPath)
        {
            _connectionString = $"Data Source={dbPath};Version=3;";
            EnsureDatabaseCreated();
        }

        private void EnsureDatabaseCreated()
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Execute(@"
                CREATE TABLE IF NOT EXISTS Stations (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT NOT NULL,
                    address TEXT NOT NULL,
                    numberOfBicyclesHold INT NOT NULL
                )");
        }

        public async Task<List<Station>> GetAllStationsAsync()
        {
            using var connection = new SQLiteConnection(_connectionString);
            var stations = await connection.QueryAsync<Station>("SELECT * FROM Stations");
            return stations.AsList();
        }

        public async Task AddStationAsync(Station station)
        {
            using var connection = new SQLiteConnection(_connectionString);
            await connection.ExecuteAsync(
                "INSERT INTO Stations (name, address, numberOfBicyclesHold) VALUES (@Name, @Address, @NumberOfBicyclesHold)",
                station);
        }

        public async Task DeleteStationAsync(int id)
        {
            using var connection = new SQLiteConnection(_connectionString);
            await connection.ExecuteAsync("DELETE FROM Stations WHERE id = @Id", new { Id = id });
        }
    }
}