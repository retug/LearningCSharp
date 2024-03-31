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
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace etabsRevitCnx
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public UIDocument uidoc { get; }
        public Document doc { get; }

        public MainWindow(UIDocument UiDoc)
        {
            uidoc = UiDoc;

            doc = UiDoc.Document;
            Title = "ETABs Revit Connection";
            InitializeComponent(); 
            DataContext = new MainViewModel(uidoc);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
