using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System;
using Dapper;
using BikeSystemAdminPanel.Models;

namespace BikeSystemAdminPanel.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    public string _SQLiteVersion = "SQLite version is unknown";

    [ObservableProperty]
    public ObservableCollection<Station> _stations = [new Station{Id = 1}];

    [RelayCommand]
    public void CreateSchemaAndGetStation()
    {
        using (var connection = new SQLiteConnection("DataSource =:memory:"))
        {
            //_stations.Clear();
            connection.Open();

            var res = connection.QueryFirst("SELECT sqlite_version() AS Version;");
            SQLiteVersion = res.Version;

            // stations
            var createTableSql = @"
                CREATE TABLE IF NOT EXISTS Stations (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    address TEXT NOT NULL,
                    numberOfBicyclesHold INT NOT NULL
                );
            ";
            connection.Execute(createTableSql);

            // bicycles
            createTableSql = @"
                CREATE TABLE IF NOT EXISTS Bicycles (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT NOT NULL,
                    type TEXT NOT NULL,
                    stationId INT NOT NULL,
                    FOREIGN KEY (stationId) REFERENCES Stations (id)
                );
            ";
            connection.Execute(createTableSql);

            //users
            createTableSql = @"
                CREATE TABLE IF NOT EXISTS Users (
                    phoneNumber TEXT PRIMARY KEY,
                    name TEXT NOT NULL,
                    surname TEXT NOT NULL
                );
            ";
            connection.Execute(createTableSql);

            //rentals
            createTableSql = @"
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
                );
            ";
            connection.Execute(createTableSql);

            var station = new Station{Id = 1, Address = "test address", NumberOfBicyclesHold = 0};
            var bicyleInsert = new Bicycle{Id = 1, Name = "Test Nazwa", Type = "Test Typ", StationId = 1};

            var insertBicylesSql = @"INSERT INTO Bicycles (id, name, type, stationId) VALUES (@Id, @Name, @Type, @StationId);";
            var insertStationsSql = @"INSERT INTO Stations (id, address, numberOfBicyclesHold) VALUES (@Id, @Address, @NumberOfBicyclesHold);";
            var rowsAffectedStations = connection.Execute(insertStationsSql, station);
            var rowsAffected = connection.Execute(insertBicylesSql, bicyleInsert);

            var selectStationsSql = "SELECT * FROM Stations";
            var result = connection.Query<Station>(selectStationsSql);
            foreach (var stationResult in result) {
                Stations.Add(station);
            }
        }
    }
}
