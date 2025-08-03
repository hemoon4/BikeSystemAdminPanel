using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading.Tasks;
using BikeSystemAdminPanel.Models;
using Dapper;

namespace BikeSystemAdminPanel.Database
{
    public class SqliteRentalRepository : IRentalRepository
    {
        private readonly string _connectionString;

        public SqliteRentalRepository(string dbPath)
        {
            _connectionString = $"Data Source={dbPath};Version=3;";
            EnsureDatabaseCreated();
        }

        private void EnsureDatabaseCreated()
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Execute(@"
                CREATE TABLE IF NOT EXISTS Rentals (
                    id INTEGER PRIMARY KEY,
                    startTime DATETIME NOT NULL,
                    endTime DATETIME,
                    userPhoneNumber TEXT NOT NULL,
                    stationId INT NOT NULL,
                    bicycleId INT NOT NULL,
                    FOREIGN KEY (userPhoneNumber) REFERENCES Users(phoneNumber),
                    FOREIGN KEY (stationId) REFERENCES Stations(id),
                    FOREIGN KEY (bicycleId) REFERENCES Bicycles(id)
                )");
        }

        public async Task<List<Rental>> GetAllRentalsAsync()
        {
            using var connection = new SQLiteConnection(_connectionString);
            var rentals = await connection.QueryAsync<Rental>("SELECT * FROM Rentals");
            return rentals.AsList();
        }

        public async Task<int> AddRentalAsync(Rental rental)
        {
            using var connection = new SQLiteConnection(_connectionString);
            return await connection.ExecuteScalarAsync<int>(
                "INSERT INTO Rentals (startTime, endTime, userPhoneNumber, stationId, bicycleId) VALUES (@StartTime, @EndTime, @UserPhoneNumber, @StationId, @BicycleId); SELECT last_insert_rowid();",
                rental);
        }

        public async Task DeleteRentalAsync(int id)
        {
            using var connection = new SQLiteConnection(_connectionString);
            await connection.ExecuteAsync("DELETE FROM Rentals WHERE id = @Id", new { Id = id });
        }
    }
}