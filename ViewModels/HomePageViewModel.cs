using System;
using System.Linq;
using System.IO; 
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkiaSharp;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using BikeSystemAdminPanel.Database;
using BikeSystemAdminPanel.Services;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace BikeSystemAdminPanel.ViewModels
{
    public partial class HomePageViewModel : ViewModelBase
    {
        private readonly IRentalRepository _repository;

        public HomePageViewModel()
        {
            var dbPath = "data.db";
            _repository = new SqliteRentalRepository(dbPath);
        }

        [ObservableProperty]
        private List<ISeries> _series;

        [ObservableProperty]
        private List<Axis> _xAxes;

        [ObservableProperty]
        private List<Axis> _yAxes;

        [ObservableProperty]
        private string? _errorMessages;

        private DateTime[] _chartDates;
        private int[] _chartValues;

        public async Task LoadChart()
        {
            var rentals = await _repository.GetAllRentalsAsync().ConfigureAwait(false);

            var now = DateTime.Now;
            var firstDay = new DateTime(now.Year, now.Month, 1);
            var lastDay = firstDay.AddMonths(1).AddDays(-1);

            var thisMonthRentals = rentals.Where(r => r.StartTime >= firstDay && r.StartTime <= lastDay);
            var grouped = thisMonthRentals.GroupBy(r => r.StartTime.Date)
                                         .Select(g => new { Date = g.Key, Count = g.Count() })
                                         .OrderBy(x => x.Date);

            var daysInMonth = lastDay.Day;
            _chartValues = new int[daysInMonth];
            _chartDates = new DateTime[daysInMonth];

            // Initialize dates for all days in the month
            for (int i = 0; i < daysInMonth; i++)
            {
                _chartDates[i] = firstDay.AddDays(i);
            }

            // Populate chart values
            foreach (var g in grouped)
            {
                int dayIndex = g.Date.Day - 1;
                _chartValues[dayIndex] = g.Count;
            }

            Series = 
            [
                new ColumnSeries<int>
                {
                    Values = _chartValues,
                    Name = "Rentals per Day"
                }
            ];

            XAxes = [
                new Axis
                {
                    Labels = Enumerable.Range(1, daysInMonth).Select(d => d.ToString()).ToList(),
                    Name = "Day of Month",
                    TextSize = 12,
                    ShowSeparatorLines = true
                }
            ];

            YAxes = [
                new Axis
                {
                    Name = "Number of Rentals",
                    TextSize = 12,
                    ShowSeparatorLines = true,
                    MinLimit = 0
                }
            ];
        }

        [RelayCommand]
        private async Task ExportToCsv()
        {
            try
            {
                var filesService = App.Current?.Services?.GetService<IFilesService>();
                if (filesService is null) throw new NullReferenceException("Missing File Service instance.");

                var file = await filesService.SaveFileAsync();
                if (file is null) return;

                var csv = new StringBuilder();
                csv.AppendLine("Date,Number of Rentals");
                for (int i = 0; i < _chartDates.Length; i++)
                {
                    csv.AppendLine($"{_chartDates[i]:yyyy-MM-dd},{_chartValues[i]}");
                }

                var stream = new MemoryStream(Encoding.Default.GetBytes(csv.ToString()));
                await using var writeStream = await file.OpenWriteAsync();
                await stream.CopyToAsync(writeStream);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}