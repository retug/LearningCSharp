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

namespace PanandZoom
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //RAM Canvas Factors
        private double ramZoomFactor = 1.0;
        private System.Windows.Point ramLastMousePosition;
        private List<System.Windows.Shapes.Line> ramSelectedLines = new List<System.Windows.Shapes.Line>();

        public MainWindow()
        {
            InitializeComponent();
            // Attach the PreviewMouseWheel event to handle zoom
            scrollViewer.PreviewMouseWheel += ramScrollViewer_PreviewMouseWheel;
            // Attach mouse events for panning
            scrollViewer.MouseLeftButtonDown += ramCanvas_MouseLeftButtonDown;
            scrollViewer.MouseMove += ramCanvas_MouseMove;
            scrollViewer.MouseLeftButtonUp += ramCanvas_MouseLeftButtonUp;

        }
        private void ramScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true; // Prevent standard scrolling

            Point mousePos = e.GetPosition(scrollViewer);

            if (e.Delta > 0)
            {
                ramZoomFactor *= 1.2;
            }
            else
            {
                ramZoomFactor /= 1.2;
            }

            // Apply the zoom factor to the canvas content
            ramCanvas.LayoutTransform = new ScaleTransform(ramZoomFactor, ramZoomFactor);

            // Adjust the scroll position to zoom in/out around the mouse position
            scrollViewer.ScrollToHorizontalOffset(mousePos.X * ramZoomFactor - mousePos.X);
            scrollViewer.ScrollToVerticalOffset(mousePos.Y * ramZoomFactor - mousePos.Y);
        }
        private void ramCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //PANNING FUNCTION
            ramLastMousePosition = e.GetPosition(scrollViewer);
            scrollViewer.CaptureMouse();
            coordinatesTextBlock.Text = $"RAM - X: {ramLastMousePosition.X}, Y: {ramLastMousePosition.Y}";
        }

        private void ramCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            scrollViewer.ReleaseMouseCapture();
        }

        private void ramCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            //Panning function with mouse down event
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                System.Windows.Point position = e.GetPosition(scrollViewer);
                double offsetX = position.X - ramLastMousePosition.X;
                double offsetY = position.Y - ramLastMousePosition.Y;

                // Update the position of the canvas content
                var transform = ramCanvas.RenderTransform as TranslateTransform ?? new TranslateTransform();

                transform.X += offsetX;
                transform.Y += offsetY;
                ramCanvas.RenderTransform = transform;

                ramLastMousePosition = position;
                // Update TextBlock with coordinates
                coordinatesTextBlock.Text = $"RAM - X: {ramLastMousePosition.X}, Y: {ramLastMousePosition.Y}";
                pointTextBlock.Text = $"position - X: {position.X}, Y: {position.Y}";
            }
        }
    }
}