using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Data.SQLite;
using System;
using Dapper;
using BikeSystemAdminPanel.Models;

namespace BikeSystemAdminPanel.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    public void GetVersion(object sender, RoutedEventArgs args)
    {
        using (var connection = new SQLiteConnection("DataSource =:memory:"))
        {
            connection.Open();
            var res = connection.QueryFirst("SELECT sqlite_version() AS Version;");

            SQLiteVersionText.Text = res.Version;
            var createTableSql = @"
                CREATE TABLE IF NOT EXISTS Bicycles (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT NOT NULL,
                    type TEXT NOT NULL
                );
            ";
            connection.Execute(createTableSql);
            var bicyleInsert = new Bicycle{Id = 1, Name = "Test Nazwa", Type = "Test Typ"};
            var insertBicylesSql = @"INSERT INTO Bicycles (id, name, type) VALUES (@Id, @Name, @Type);";
            var rowsAffected = connection.Execute(insertBicylesSql, bicyleInsert);

            var selectBicyclesSql = "SELECT * FROM Bicycles";
            var bicycles = connection.Query<Bicycle>(selectBicyclesSql);
            Bicycles.ItemsSource = bicycles;
        }
    }
}