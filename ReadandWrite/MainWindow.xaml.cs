using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
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
using Newtonsoft.Json;
using Microsoft.Win32;
using Microsoft.VisualBasic;

namespace SaveandRead
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    static class LineExtensions
    {
        public static void GetDirectionAndAngle(this System.Windows.Shapes.Line line, out Vector direction, out double angle)
        {
            Point startPoint = new Point(line.X1, line.Y1);
            Point endPoint = new Point(line.X2, line.Y2);

            //Direction is a vector direction of the line aka, (1,0) is a vector going to the right

            direction = endPoint - startPoint;

            angle = Math.Atan2(direction.Y, direction.X) * (180 / Math.PI);
        }
    }



    public partial class MainWindow : Window
    {
        // Create properties for data binding
        public decimal XValue { get; set; }
        public decimal YValue { get; set; }
        public decimal RotValue { get; set; }

        System.Windows.Shapes.Line lineRAM1 = new System.Windows.Shapes.Line
        {
            //X1 = 0,
            //Y1 = 0,
            //X2 = 10,
            //Y2 = 0,
            X1 = 0,
            Y1 = 0,
            X2 = 1,
            Y2 = 1,
            Stroke = Brushes.Black,
            StrokeThickness = 2,
            Name = "A" // Adding the Name attribute
        };

        System.Windows.Shapes.Line lineRAM2 = new System.Windows.Shapes.Line
        {
            X1 = 0,
            Y1 = 0,
            X2 = -1,
            Y2 = 1,
            Stroke = Brushes.Black,
            StrokeThickness = 2,
            Name = "L1" // Adding the Name attribute
        };

    System.Windows.Shapes.Line lineRevit1 = new System.Windows.Shapes.Line
        {
            X1 = 0,
            Y1 = 0,
            X2 = 0,
            Y2 = 10,
            Stroke = Brushes.Black,
            StrokeThickness = 2,
            Name = "A" // Adding the Name attribute
        };

        System.Windows.Shapes.Line lineRevit2 = new System.Windows.Shapes.Line
        {
            X1 = 0,
            Y1 = 0,
            X2 = -10,
            Y2 = 0,
            Stroke = Brushes.Black,
            StrokeThickness = 2,
            Name = "L1" // Adding the Name attribute
        };



public MainWindow()
        {
            InitializeComponent();
            

            // Find intersection point
            Point intersectionRAM = FindIntersection(
                new Point(lineRAM1.X1, lineRAM1.Y1),
                new Point(lineRAM1.X2, lineRAM1.Y2),
                new Point(lineRAM2.X1, lineRAM2.Y1),
                new Point(lineRAM2.X2, lineRAM2.Y2)
            );

            // Find intersection point Revit
            Point intersectionREVIT = FindIntersection(
                new Point(lineRevit1.X1, lineRevit1.Y1),
                new Point(lineRevit1.X2, lineRevit1.Y2),
                new Point(lineRevit2.X1, lineRevit2.Y1),
                new Point(lineRevit2.X2, lineRevit2.Y2)
            );

            // Calculate offset
            double offsetX = intersectionREVIT.X - intersectionRAM.X;
            double offsetY = intersectionREVIT.Y - intersectionREVIT.Y;

            XTextBox.Text = offsetX.ToString();
            YTextBox.Text = offsetY.ToString();
            

            // Determine vector direction and angle using the extension method
            Vector directionRAM1;
            double angleRAM1;
            lineRAM1.GetDirectionAndAngle(out directionRAM1, out angleRAM1);

            Vector directionRAM2;
            double angleRAM2;
            lineRAM2.GetDirectionAndAngle(out directionRAM2, out angleRAM2);

            // Determine vector direction and angle using the extension method
            Vector directionREVIT1;
            double angleREVIT1;
            lineRevit1.GetDirectionAndAngle(out directionREVIT1, out angleREVIT1);

            Vector directionREVIT2;
            double angleREVIT2;
            lineRevit2.GetDirectionAndAngle(out directionREVIT2, out angleREVIT2);

            //the value of x is the global rotation parameter
            double rot = FindRotation(directionRAM1, directionRAM2, directionREVIT1, directionREVIT2, angleRAM1, angleRAM2, angleREVIT1, angleREVIT2, lineRAM1, lineRAM2, lineRevit1, lineRevit2);

            RotTextBox.Text = rot.ToString();

            // Bind the event handler to the TextChanged event of the TextBox controls.
            XTextBox.TextChanged += TextBox_TextChanged;
            YTextBox.TextChanged += TextBox_TextChanged;
            RotTextBox.TextChanged += TextBox_TextChanged;
        }

        

        // Function to find the intersection point of two lines
        static Point FindIntersection(Point p1, Point p2, Point p3, Point p4)
        {
            double x1 = p1.X, y1 = p1.Y;
            double x2 = p2.X, y2 = p2.Y;
            double x3 = p3.X, y3 = p3.Y;
            double x4 = p4.X, y4 = p4.Y;

            double ua = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) /
                        ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));

            double x = x1 + ua * (x2 - x1);
            double y = y1 + ua * (y2 - y1);

            return new Point(x, y);
        }

        //it would be nice to re-write this function in the future to extend the line class to store all of this data in the line.

        static double FindRotation(Vector RAM1V, Vector RAM2V, Vector REVIT1V, Vector REVIT2V, double RAM1angle, double RAM2angle, double REVIT1angle, double REVIT2angle, Line RAM1Line, Line RAM2Line, Line REVIT1Line, Line REVIT2Line)
        {
            double RAMdelta = RAM2angle - RAM1angle;
            double Revitdelta = REVIT2angle - REVIT1angle;

            double rotationR2R = 0;

            if (RAMdelta - Revitdelta <= 1)
            {
                System.Console.WriteLine("rotation between angles is about the same");
                if (RAM1Line.Name == REVIT1Line.Name)
                {
                    rotationR2R = RAM1angle - REVIT1angle;
                }
                else if (RAM1Line.Name == REVIT2Line.Name)
                {
                    rotationR2R = RAM1angle - REVIT2angle;
                }
                else
                {
                    //this could be more elegant, this is a bit sloppy if the gridlines between revit and ram do not match, might have to prompt user for override.
                    rotationR2R = RAM1angle - REVIT1angle;
                }
            }
            else
            {
                System.Console.WriteLine("your angles between ram and revit do not match");
            }
            return rotationR2R;
        }

        


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string inputText = textBox.Text;

            if (!IsDecimal(inputText))
            {
                // If the input is not a valid negative decimal, display an error message.
                textBox.Background = new SolidColorBrush(Colors.Red);
                

            }
            else
            {
                textBox.Background = SystemColors.WindowBrush;

                // Update XValue or YValue based on the TextBox that triggered the event
                if (textBox == XTextBox)
                {
                    XValue = decimal.Parse(inputText);
                }
                else if (textBox == YTextBox)
                {
                    YValue = decimal.Parse(inputText);
                }
                else if (textBox == RotTextBox)
                {
                    RotValue = decimal.Parse(inputText);
                }
            }
        }

        // Helper function to check if a string can be parsed as a negative decimal.
        private bool IsDecimal(string input)
        {
            // Attempt to parse the input as a negative decimal.
            if (decimal.TryParse(input, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out decimal result))
            {
                // Check if the result is less than zero, indicating a negative decimal.
                return true;

            }

            return false; // Not a decimal.
        }

        private void SaveJson()
        {

            // Check if both XTextBox and YTextBox contain valid decimal values
            if (!IsDecimal(XTextBox.Text) || !IsDecimal(YTextBox.Text) || !IsDecimal(RotTextBox.Text))
            {
                MessageBox.Show("Please enter valid decimal values for X,Y, and rotation before saving.");
                return;
            }

            // Read the current values of XValue and YValue
            decimal currentXValue = XValue;
            decimal currentYValue = YValue;
            decimal currentRotValue = RotValue;


            // Create an instance of a class to hold XValue and YValue
            var data = new { XValue = currentXValue, YValue = currentYValue, RotValue = currentRotValue };


            // Convert the object to a JSON string
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);

            // Create a SaveFileDialog
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                Title = "Save R2R mapping file",
                FileName = "R2R_Map.json"
            };

            // Show the SaveFileDialog
            if (saveFileDialog.ShowDialog() == true)
            {
                // Get the selected file path from the SaveFileDialog
                string filePath = saveFileDialog.FileName;

                // Write the JSON string to the selected file
                File.WriteAllText(filePath, json);

                MessageBox.Show("R2R data saved successfully!");
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveJson();
        }

        private void LoadJson()
        {
            // Create an OpenFileDialog
            var openFileDialog = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                Title = "Open JSON File"
            };

            // Show the OpenFileDialog
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Read the JSON file
                    string json = File.ReadAllText(openFileDialog.FileName);

                    // Deserialize the JSON string to an anonymous type with XValue and YValue properties
                    var data = JsonConvert.DeserializeAnonymousType(json, new { XValue = default(decimal), YValue = default(decimal), RotValue = default(decimal) });

                    // Set the values to XValue and YValue
                    XValue = data.XValue;
                    YValue = data.YValue;
                    RotValue = data.RotValue;
                    // Update the TextBox controls with the loaded values
                    XTextBox.Text = XValue.ToString();
                    YTextBox.Text = YValue.ToString();
                    RotTextBox.Text = RotValue.ToString();

                    MessageBox.Show("JSON data loaded successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading JSON file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            LoadJson();
        }

    }
}


