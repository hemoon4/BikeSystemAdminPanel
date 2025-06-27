using System;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using SkiaSharp;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using BikeSystemAdminPanel.Database;
using System.Collections.ObjectModel;
using LiveChartsCore.Drawing;
using LiveChartsCore.Measure;
using System.Collections.Generic;

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
            var values = new int[daysInMonth];
            
            foreach (var g in grouped)
            {
                values[g.Date.Day - 1] = g.Count;
            }

            Series = 
            [
                new ColumnSeries<int>
                {
                    Values = values,
                    Name = "Rentals per Day"
                }
            ];
            // FIXME: WHY AXIS DON'T SHOW???????
            XAxes = [
                new Axis
                {
                    Labels = Enumerable.Range(1, daysInMonth).Select(d => d.ToString()).ToArray(),
                    Name = "Day of Month",
                    // LabelsPaint = new SolidColorPaint(SKColors.Black),
                    // NamePaint = new SolidColorPaint(SKColors.Red),
                    // TextSize = 12,
                    // NamePadding = new Padding(10),
                    // NameTextSize = 14,
                    // Padding = new Padding(10),
                    // LabelsDensity = 0,
                    // Position = AxisPosition.Start,
                    // ShowSeparatorLines = true,
                    // SeparatorsAtCenter = true,
                    // IsVisible = true
                }
            ];

            YAxes = [

                new Axis
                {
                    Labels = Enumerable.Range(1, daysInMonth).Select(d => d.ToString()).ToArray(),
                    Name = "Number of Rentals",
                    // LabelsPaint = new SolidColorPaint(SKColors.Red),
                    // NamePaint = new SolidColorPaint(SKColors.Red),
                    // NamePadding = new Padding(10),
                    // TextSize = 12,
                    // NameTextSize = 14,
                    // Padding = new Padding(10),
                    // LabelsDensity = 0,
                    // Position = AxisPosition.Start,
                    // ShowSeparatorLines = true,
                    // SeparatorsAtCenter = true,
                    // IsVisible = true
                }
            ];
        }
    }
}