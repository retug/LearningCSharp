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
using System.Collections.Generic;
using System.Linq;

namespace WpfApp1;


public class Data
{
    public void ProcessData()
    {
        List<double> testX = new List<double> { 0, 20, 0, 20 };
        List<double> testY = new List<double> { 0, 0, 20, 20 };

        for (int i = 0; i < testX.Count(); i++)
        {

        }
    }
    
              
}


public class ViewModel : ObservableObject
{
    List<double> testX = new List<double> { 0, 20, 0, 20 };
    List<double> testY = new List<double> { 0, 0, 20, 20 };

    LiveChartsCore.Defaults.ObservablePoint test = new LiveChartsCore.Defaults.ObservablePoint();
    
    



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
    // https://livecharts.dev/docs/WPF/2.0.0-rc1/samples.general.sections2


    public ISeries[] Series3 { get; set; } =
    {
        new LineSeries<double>
        {
            
            //ChartValues<LiveChartsCore.Defaults.ObservablePoint> areaPoints = new LiveCharts.ChartValues<LiveCharts.Defaults.ObservablePoint>();
        }
    };


}

