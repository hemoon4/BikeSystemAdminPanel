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
        private readonly IRentalRepository _rentalRepository;
        private readonly IStationRepository _stationRepository;

        public HomePageViewModel()
        {
            var dbPath = "data.db";
            _rentalRepository = new SqliteRentalRepository(dbPath);
            _stationRepository = new SqliteStationRepository(dbPath);
        }

        [ObservableProperty]
        private List<ISeries> _series;

        [ObservableProperty]
        private List<Axis> _xAxes;

        [ObservableProperty]
        private List<Axis> _yAxes;

        [ObservableProperty]
        private string? _errorMessages;

        [ObservableProperty]
        private List<string> _chartTypes = new List<string>
        {
            "This Month Rentals per Day",
            "This Month Rentals per Station"
        };

        [ObservableProperty]
        private string _selectedChartType = "This Month Rentals per Day";

        private DateTime[] _chartDates;
        private int[] _chartValues;
        private string[] _chartLabels;

        public async Task LoadChart()
        {
            try
            {
                var rentals = await _rentalRepository.GetAllRentalsAsync().ConfigureAwait(false);

                var now = DateTime.Now;
                var firstDay = new DateTime(now.Year, now.Month, 1);
                var lastDay = firstDay.AddMonths(1).AddDays(-1);

                var thisMonthRentals = rentals.Where(r => r.StartTime >= firstDay && r.StartTime <= lastDay).ToList();
                if (!thisMonthRentals.Any())
                {
                    Series = [ new ColumnSeries<int> { Values = new int[] { 0 }, Name = "No Data" } ];
                    XAxes = [ new Axis { Labels = new[] { "N/A" }.ToList(), Name = "No Data" } ];
                    YAxes = [ new Axis { Name = "Number of Rentals", MinLimit = 0 } ];
                    return;
                }

                if (SelectedChartType == "This Month Rentals per Day")
                {
                    var grouped = thisMonthRentals.GroupBy(r => r.StartTime.Date)
                                                 .Select(g => new { Date = g.Key, Count = g.Count() })
                                                 .OrderBy(x => x.Date)
                                                 .ToList();

                    var daysInMonth = lastDay.Day;
                    _chartValues = new int[daysInMonth];
                    _chartDates = new DateTime[daysInMonth];

                    for (int i = 0; i < daysInMonth; i++)
                    {
                        _chartDates[i] = firstDay.AddDays(i);
                    }

                    foreach (var g in grouped)
                    {
                        Console.WriteLine($"Date: {g.Date}, Count: {g.Count}");
                        int dayIndex = g.Date.Day - 1;
                        _chartValues[dayIndex] = g.Count;
                    }

                    Series =
                    [
                        new ColumnSeries<int>
                        {
                            Values = _chartValues,
                            Name = "Rentals per Day",
                            Fill = new SolidColorPaint(SKColors.Blue)
                        }
                    ];

                    XAxes = [
                        new Axis
                        {
                            Labels = Enumerable.Range(1, daysInMonth).Select(d => d.ToString()).ToList(),
                            Name = "Day of Month",
                            LabelsPaint = new SolidColorPaint(SKColors.Black),
                            NamePaint = new SolidColorPaint(SKColors.Black),
                            TextSize = 12,
                            NameTextSize = 14,
                            ShowSeparatorLines = true
                        }
                    ];
                }
                else
                {
                    var stations = await _stationRepository.GetAllStationsAsync().ConfigureAwait(false);
                    var grouped = thisMonthRentals.GroupBy(r => r.StationId)
                                                 .Select(g => new
                                                 {
                                                     StationId = g.Key,
                                                     Count = g.Count(),
                                                     StationName = stations.FirstOrDefault(s => s.Id == g.Key)?.Name ?? $"Station {g.Key}"
                                                 })
                                                 .ToList();

                    _chartValues = grouped.Select(g => g.Count).ToArray();
                    _chartLabels = grouped.Select(g => g.StationName).ToArray();

                    Series =
                    [
                        new ColumnSeries<int>
                        {
                            Values = _chartValues,
                            Name = "Rentals per Station",
                            Fill = new SolidColorPaint(SKColors.Green)
                        }
                    ];

                    XAxes = [
                        new Axis
                        {
                            Labels = _chartLabels.ToList(),
                            Name = "Station",
                            LabelsPaint = new SolidColorPaint(SKColors.Black),
                            NamePaint = new SolidColorPaint(SKColors.Black),
                            TextSize = 12,
                            NameTextSize = 14,
                            ShowSeparatorLines = true
                        }
                    ];
                }

                YAxes = [
                    new Axis
                    {
                        Labeler = value => value.ToString("N0"),
                        Name = "Number of Rentals",
                        LabelsPaint = new SolidColorPaint(SKColors.Black),
                        NamePaint = new SolidColorPaint(SKColors.Black),
                        TextSize = 12,
                        NameTextSize = 14,
                        ShowSeparatorLines = true,
                        MinLimit = 0
                    }
                ];

                ErrorMessages = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoadChart: {ex.Message}");
                ErrorMessages = ex.Message;
            }
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
                if (SelectedChartType == "This Month Rentals per Day")
                {
                    csv.AppendLine("Date,Number of Rentals");
                    for (int i = 0; i < _chartDates.Length; i++)
                    {
                        csv.AppendLine($"{_chartDates[i]:yyyy-MM-dd},{_chartValues[i]}");
                    }
                }
                else
                {
                    csv.AppendLine("Station, Number of Rentals");
                    for (int i = 0; i < _chartLabels.Length; i++)
                    {
                        csv.AppendLine($"{_chartLabels[i]}, {_chartValues[i]}");
                    }
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