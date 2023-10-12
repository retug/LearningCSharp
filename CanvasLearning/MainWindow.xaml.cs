using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double zoomFactor = 1.0;
        private Point lastMousePosition;
        private List<Line> selectedLines = new List<Line>();
        public MainWindow()
        {
            InitializeComponent();

            // Attach the PreviewMouseWheel event to handle zoom
            scrollViewer.PreviewMouseWheel += ScrollViewer_PreviewMouseWheel;

            // Attach mouse events for panning
            canvas.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            canvas.MouseMove += Canvas_MouseMove;
            canvas.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;

            CustomLine lineC1 = new CustomLine(new Point(0, 0), new Point(10, 10), "A");
            CustomLine lineC2 = new CustomLine(new Point(5, 5), new Point(5, 15), "B");
            CustomLine lineC3 = new CustomLine(new Point(0, 0), new Point(10, 0), "C");

            List<CustomLine> MyLines = new List<CustomLine>();
            MyLines.Add(lineC1);   
            MyLines.Add(lineC2);
            MyLines.Add(lineC3);

            foreach (var line in MyLines)
            {
                Line lineX = new Line
                {
                    X1 = line.StartPoint.X,
                    Y1 = line.StartPoint.Y,
                    X2 = line.EndPoint.X,
                    Y2 = line.EndPoint.Y,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                    Tag = line // Set the Tag property to the CustomLine object, need to read up on Tag
                };
                canvas.Children.Add(lineX);
                lineX.MouseEnter += Line_MouseEnter;
                lineX.MouseLeave += Line_MouseLeave;
                AddCircleWithText(line.EndPoint.X, line.EndPoint.Y, line.Name);

            }
            AdjustZoomToFitLines();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true; // Prevent standard scrolling

            if (e.Delta > 0)
            {
                // Zoom in
                zoomFactor *= 1.2; // You can adjust the zoom factor as needed
            }
            else
            {
                // Zoom out
                zoomFactor /= 1.2;
            }

            // Apply the zoom factor to the canvas content
            canvas.LayoutTransform = new ScaleTransform(zoomFactor, zoomFactor);
        }

        //TESTING FOR CLICKING ON LINES
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //PANNING FUNCTION
            lastMousePosition = e.GetPosition(scrollViewer);
            canvas.CaptureMouse();

            if (e.OriginalSource is Line line)
            {
                // Clicked on a line, add it to the selectedLines list
                if (!selectedLines.Contains(line))
                {
                    line.Stroke = Brushes.Red;
                    selectedLines.Add(line);
                    selectedLinesListBox.Items.Add(((CustomLine)line.Tag).Name); // Add the name to the list, pure chatgpt here
                }
            }
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            canvas.ReleaseMouseCapture();
        }

        //END PANNING FUNCTION

        private void ClearSelection_Click(object sender, RoutedEventArgs e)
        {
            foreach (Line line in selectedLines)
            {
                line.Stroke = Brushes.Black;
            }
            selectedLines.Clear();
            selectedLinesListBox.Items.Clear(); // Clear the list of selected lines
        }

        private void ClearSelection_Click1(object sender, RoutedEventArgs e)
        {
            foreach (Line line in selectedLines)
            {
                line.Stroke = Brushes.Black;
            }
            selectedLines.Clear();
            selectedLinesListBox.Items.Clear(); // Clear the list of selected lines
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (canvas.IsMouseCaptured)
            {
                Point position = e.GetPosition(scrollViewer);
                double offsetX = position.X - lastMousePosition.X;
                double offsetY = position.Y - lastMousePosition.Y;

                // Update the position of the canvas content
                var transform = canvas.RenderTransform as TranslateTransform ?? new TranslateTransform();
                transform.X += offsetX;
                transform.Y += offsetY;
                canvas.RenderTransform = transform;

                lastMousePosition = position;
            }
        }


        private void AddCircleWithText(double x, double y, string text)
        {
            Grid container = new Grid();

            Ellipse circle = new Ellipse
            {
                Width = 10, // Diameter = 2 * Radius
                Height = 10, // Diameter = 2 * Radius
                Fill = Brushes.Red,
                Stroke = Brushes.Black,
                StrokeThickness = 1,
            };

            TextBlock textBlock = new TextBlock
            {
                Text = text,
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            container.Children.Add(circle);
            container.Children.Add(textBlock);

            // Position the container at (x, y)
            Canvas.SetLeft(container, x - 5); // Adjust for the radius
            Canvas.SetTop(container, y - 5); // Adjust for the radius

            canvas.Children.Add(container);
        }

        private void Line_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Line line)
            {
                // Change line color to red when hovered over
                line.Stroke = Brushes.Red;
            }
        }

        private void Line_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Line line)
            {
                // Change line color back to black when the mouse leaves
                line.Stroke = Brushes.Black;
            }
        }

        private void AdjustZoomToFitLines()
        {
            // Calculate the maximum X and Y coordinates of your lines
            double maxX = double.MinValue;
            double maxY = double.MinValue;

            foreach (Line line in canvas.Children.OfType<Line>())
            {
                maxX = Math.Max(maxX, Math.Max(line.X1, line.X2));
                maxY = Math.Max(maxY, Math.Max(line.Y1, line.Y2));
            }

            // Calculate the desired center point
            double centerX = maxX / 2;
            double centerY = maxY / 2;

            // Calculate the zoom factor required to fit the entire content within the canvas view
            double canvasWidth = canvas.ActualWidth;
            double canvasHeight = canvas.ActualHeight;

            double zoomX = canvasWidth / maxX;
            double zoomY = canvasHeight / maxY;

            double zoomFactor = Math.Min(zoomX, zoomY);

            // Apply the calculated zoom factor and center point to the canvas
            canvas.LayoutTransform = new ScaleTransform(zoomFactor, zoomFactor);
            canvas.RenderTransform = new TranslateTransform(centerX - canvasWidth / 2, centerY - canvasHeight / 2);
        }
    }
}
