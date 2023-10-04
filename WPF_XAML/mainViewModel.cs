using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;
using System.Collections.Generic;
using System;

using System.Linq;
using LiveChartsCore.Kernel.Sketches;


namespace WpfApp1;


public class Data
{
    public void ProcessData(ViewModel ViewModel)
    {
        List<double> testX = new List<double> { 0, 20, 0, 20 };
        List<double> testY = new List<double> { 0, 0, 20, 20 };
        List<string> names = new List<string> { "A", "B", "A", "B" };

        //LiveChartsCore.SkiaSharpView.WPF.CartesianChart gridPlot = new LiveChartsCore.SkiaSharpView.WPF.CartesianChart();

        List<LineSeries<ObservablePoint>> GridSeries = new List<LineSeries<ObservablePoint>>();
        

        for (int i = 0; i < testX.Count()/2; i++)
        {
            var areaPoints = new List<ObservablePoint>();

            ObservablePoint testStart = new LiveChartsCore.Defaults.ObservablePoint();
            testStart.X = testX[i*2];
            testStart.Y = testY[i*2];
            ObservablePoint testEnd= new LiveChartsCore.Defaults.ObservablePoint();
            testEnd.X = testX[i*2+1];
            testEnd.Y = testY[i*2+1];

            areaPoints.Add(testStart);
            areaPoints.Add(testEnd);

            var lineSeries = new LineSeries<ObservablePoint>
            {
                Values = areaPoints,
                Name = names[i],
                Fill = null
            };
            GridSeries.Add(lineSeries);
            

        }
        ViewModel.GridSeries = GridSeries;

    }
}


public class ViewModel : ObservableObject
{
    public ViewModel() //constructor of view model
    {
        GridSeries = new List<LineSeries<ObservablePoint>>();
        // Create an instance of the Data class
        Data dataProcessor = new Data();

        // Call the ProcessData method to populate GridSeries
        dataProcessor.ProcessData(this);
    }

    public ISeries[] Series { get; set; } =
    {
        new LineSeries<double>
        {
            Values = new double[] { 2, 1, 3, 5, 3, 4, 6 },
            Fill = null
        }
    };

    public LabelVisual Title { get; set; } =
        new LabelVisual
        {
            Text = "My chart title",
            TextSize = 25,
            Padding = new LiveChartsCore.Drawing.Padding(15),
            Paint = new SolidColorPaint(SKColors.DarkSlateGray)
        };

    public ISeries[] Series2 { get; set; } =
    {
        new LineSeries<double>
        {
            Values = new double[] { 1, 10, -3, 5, 3, 4, 6 },
            Fill = null,
            Stroke = new SolidColorPaint(SKColors.Red)
        }
    };

    public LabelVisual Title2 { get; set; } =
        new LabelVisual
        {
            Text = "My chart title",
            TextSize = 25,
            Padding = new LiveChartsCore.Drawing.Padding(15),
            
        };


    
    public List<LineSeries<ObservablePoint>> GridSeries { get; set; }

}

