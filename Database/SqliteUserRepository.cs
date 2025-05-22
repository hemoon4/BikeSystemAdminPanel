using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;
using Dapper;
using BikeSystemAdminPanel.Models;

namespace BikeSystemAdminPanel.Database
{
    public class SQLiteUserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public SQLiteUserRepository(string dbPath)
        {
            _connectionString = $"Data Source={dbPath}";
            EnsureTableCreated();
        }

        private void EnsureTableCreated()
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Execute(@"
                CREATE TABLE IF NOT EXISTS Users (
                    phoneNumber TEXT PRIMARY KEY,
                    name TEXT NOT NULL,
                    surname TEXT NOT NULL
                );
            ");
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            using var connection = new SQLiteConnection(_connectionString);
            await connection.OpenAsync().ConfigureAwait(false);
            return await connection.QueryAsync<User>("SELECT * FROM Users").ConfigureAwait(false);
        }

        public async Task AddUserAsync(User user)
        {
            using var connection = new SQLiteConnection(_connectionString);
            await connection.OpenAsync().ConfigureAwait(false);
            await connection.ExecuteAsync(
                "INSERT INTO Users (phoneNumber, name, surname) VALUES (@PhoneNumber, @Name, @Surname)",
                user).ConfigureAwait(false);
        }

        public async Task UpdateUserAsync(User user)
        {
            using var connection = new SQLiteConnection(_connectionString);
            await connection.OpenAsync().ConfigureAwait(false);
            await connection.ExecuteAsync(
                "UPDATE Users SET name = @Name, surname = @Surname WHERE phoneNumber = @PhoneNumber",
                user).ConfigureAwait(false);
        }

        public async Task DeleteUserAsync(string phoneNumber)
        {
            using var connection = new SQLiteConnection(_connectionString);
            await connection.OpenAsync().ConfigureAwait(false);
            await connection.ExecuteAsync(
                "DELETE FROM Users WHERE phoneNumber = @PhoneNumber",
                new { PhoneNumber = phoneNumber }).ConfigureAwait(false);
        }
    }
}
