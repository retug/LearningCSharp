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

namespace SaveandRead
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Create properties for data binding
        public decimal XValue { get; set; }
        public decimal YValue { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            // Bind the event handler to the TextChanged event of the TextBox controls.
            XTextBox.TextChanged += TextBox_TextChanged;
            YTextBox.TextChanged += TextBox_TextChanged;

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
            if (!IsDecimal(XTextBox.Text) || !IsDecimal(YTextBox.Text))
            {
                MessageBox.Show("Please enter valid decimal values for both X and Y before saving.");
                return;
            }

            // Read the current values of XValue and YValue
            decimal currentXValue = XValue;
            decimal currentYValue = YValue;
            

            // Create an instance of a class to hold XValue and YValue
            var data = new { XValue = currentXValue, YValue = currentYValue };


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
                    var data = JsonConvert.DeserializeAnonymousType(json, new { XValue = default(decimal), YValue = default(decimal) });

                    // Set the values to XValue and YValue
                    XValue = data.XValue;
                    YValue = data.YValue;
                    // Update the TextBox controls with the loaded values
                    XTextBox.Text = XValue.ToString();
                    YTextBox.Text = YValue.ToString();

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


