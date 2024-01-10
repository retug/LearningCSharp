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
            ramCanvas.MouseLeftButtonDown += ramCanvas_MouseLeftButtonDown;
            ramCanvas.MouseMove += ramCanvas_MouseMove;
            ramCanvas.MouseLeftButtonUp += ramCanvas_MouseLeftButtonUp;
            
        }
        private void ramScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true; // Prevent standard scrolling

            Point mousePos = e.GetPosition(ramCanvas);

            if (e.Delta > 0)
            {
                // Zoom in // You can adjust the zoom factor as needed
                ramZoomFactor *= 1.2;
            }
            else
            {
                // Zoom out
                ramZoomFactor /= 1.2;
            }
            // Apply the zoom factor to the canvas content
            //ramCanvas.LayoutTransform = new ScaleTransform(ramZoomFactor, ramZoomFactor);

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
            ramCanvas.CaptureMouse();
        }

        private void ramCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ramCanvas.ReleaseMouseCapture();
        }

        private void ramCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (ramCanvas.IsMouseCaptured)
            //if (e.LeftButton == MouseButtonState.Pressed)
            {
                System.Windows.Point position = e.GetPosition(scrollViewer);
                //System.Windows.Point position = e.GetPosition(this);
                double offsetX = position.X - ramLastMousePosition.X;
                double offsetY = position.Y - ramLastMousePosition.Y;

                // Update the position of the canvas content
                var transform = ramCanvas.RenderTransform as TranslateTransform ?? new TranslateTransform();
                transform.X += offsetX;
                transform.Y += offsetY;
                ramCanvas.RenderTransform = transform;

                ramLastMousePosition = position;
            }
        }
    }
}
