using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Events;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.SkiaSharpView.WPF;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfApp1;


public class Data
{
    public void ProcessData(ViewModel ViewModel)
    {
        List<double> testX = new List<double> { 0, 20, 0, 20,0,30 };
        List<double> testY = new List<double> { 0, 0, 20, 20,0, 25 };
        List<string> names = new List<string> { "A", "B", "x", "B" };
        List<LineSeries<ObservablePoint>> GridSeries = new List<LineSeries<ObservablePoint>>();
        for (int i = 0; i < testX.Count() / 2; i++)
        {
            var areaPoints = new List<ObservablePoint>();
            ObservablePoint testStart = new LiveChartsCore.Defaults.ObservablePoint();
            testStart.X = testX[i * 2];
            testStart.Y = testY[i * 2];
            ObservablePoint testEnd = new LiveChartsCore.Defaults.ObservablePoint();
            testEnd.X = testX[i * 2 + 1];
            testEnd.Y = testY[i * 2 + 1];

            areaPoints.Add(testStart);
            areaPoints.Add(testEnd);

            var lineSeries = new LineSeries<ObservablePoint>
            {
                Values = areaPoints,
                Name = names[i],
                Fill = null,
                Stroke = new SolidColorPaint(SKColors.Red)
            };
            GridSeries.Add(lineSeries);
            ViewModel.GridSeries = GridSeries;
        }
    }
}


public partial class ViewModel : ObservableObject
{
    public ViewModel() //constructor of view model
    {
        GridSeries = new List<LineSeries<ObservablePoint>>();
        // Create an instance of the Data class
        Data dataProcessor = new Data();
        // Call the ProcessData method to populate GridSeries
        dataProcessor.ProcessData(this);
        //foreach (var lineSeries in GridSeries)
        //{
        //    lineSeries.DataPointerDown += OnPointerDown;
        //}
        
    }
    public LabelVisual Title { get; set; } =
        new LabelVisual
        {
            Text = "My chart title",
            TextSize = 25,
            Padding = new LiveChartsCore.Drawing.Padding(15),
            Paint = new SolidColorPaint(SKColors.DarkSlateGray)
        };
    public List<LineSeries<ObservablePoint>> GridSeries { get; set; }
    public LineSeries<ObservablePoint> lineSeries { get; set; }

    [RelayCommand] //for some reason, when adding relay command, I had to add in public partial
    public void PointerDown(PointerCommandArgs args)
    {
        var chart = (ICartesianChartView<SkiaSharpDrawingContext>)args.Chart;
        var values = GridSeries[0];
        chart.Invalidate(); // <- ensures the canvas is redrawn after we set the fill
        values.Stroke = new SolidColorPaint(SKColors.BlueViolet);
        chart.Invalidate(); // <- ensures the canvas is redrawn after we set the fill
        // scales the UI coordinates to the corresponding data in the chart.
        var scaledPoint = chart.ScalePixelsToData(args.PointerPosition);

        Trace.WriteLine($"Pointer left {scaledPoint.X.ToString()}");
        MessageBox.Show("you pressed me");
    }

}
