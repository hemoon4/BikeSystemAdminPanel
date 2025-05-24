using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading.Tasks;
using BikeSystemAdminPanel.Models;
using Dapper;

namespace BikeSystemAdminPanel.Database
{
    public class SqliteBicycleRepository : IBicycleRepository
    {
        private readonly string _connectionString;

        public SqliteBicycleRepository(string dbPath)
        {
            _connectionString = $"Data Source={dbPath};Version=3;";
            EnsureTableCreated();
        }

        private void EnsureTableCreated()
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Execute(@"
                CREATE TABLE IF NOT EXISTS Bicycles (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT NOT NULL,
                    type TEXT NOT NULL,
                    stationId INTEGER,
                    FOREIGN KEY(stationId) REFERENCES Stations(id)
                )");
        }

        public async Task<List<Bicycle>> GetAllBicyclesAsync()
        {
            using var connection = new SQLiteConnection(_connectionString);
            var bicycles = await connection.QueryAsync<Bicycle>("SELECT * FROM Bicycles");
            return bicycles.AsList();
        }

        public async Task AddBicycleAsync(Bicycle bicycle)
        {
            using var connection = new SQLiteConnection(_connectionString);
            await connection.ExecuteAsync(
                "INSERT INTO Bicycles (Name, Type, StationId) VALUES (@Name, @Type, @StationId)",
                bicycle);
        }

        public async Task DeleteBicycleAsync(int id)
        {
            using var connection = new SQLiteConnection(_connectionString);
            await connection.ExecuteAsync("DELETE FROM Bicycles WHERE Id = @Id", new { Id = id });
        }
    }
}