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
        public MainWindow()
        {
            InitializeComponent();

            // Attach the PreviewMouseWheel event to handle zoom
            scrollViewer.PreviewMouseWheel += ScrollViewer_PreviewMouseWheel;

            // Attach mouse events for panning
            canvas.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            canvas.MouseMove += Canvas_MouseMove;
            canvas.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;

            // Line 1: (0,0) to (10,10)
            Line line1 = new Line
            {
                X1 = 0,
                Y1 = 0,
                X2 = 10,
                Y2 = 10,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };

            // Line 2: (5,5) to (5,15)
            Line line2 = new Line
            {
                X1 = 5,
                Y1 = 5,
                X2 = 5,
                Y2 = 15,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };

            // Line 3: (0,0) to (10,0)
            Line line3 = new Line
            {
                X1 = 0,
                Y1 = 0,
                X2 = 10,
                Y2 = 0,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };

            // Add lines to the canvas
            canvas.Children.Add(line1);
            canvas.Children.Add(line2);
            canvas.Children.Add(line3);

            // Attach event handlers
            line1.MouseEnter += Line_MouseEnter;
            line1.MouseLeave += Line_MouseLeave;
            line2.MouseEnter += Line_MouseEnter;
            line2.MouseLeave += Line_MouseLeave;
            line3.MouseEnter += Line_MouseEnter;
            line3.MouseLeave += Line_MouseLeave;

            // Add circles at the end of each line
            AddCircleWithText(10, 10, "A");
            AddCircleWithText(5, 15, "B");
            AddCircleWithText(10, 0, "C");
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

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            lastMousePosition = e.GetPosition(scrollViewer);
            canvas.CaptureMouse();
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

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            canvas.ReleaseMouseCapture();
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
    }
}
