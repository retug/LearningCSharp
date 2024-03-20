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
        public IEnumerable<Element> structuralFramingElements { get; set; }

        public MainWindow(UIDocument UiDoc)
        {
            uidoc = UiDoc;

            doc = UiDoc.Document;
            Title = "ETABs Revit Connection";
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void GatherBeamsButton_Click(object sender, RoutedEventArgs e)
        {
            
            this.Hide();
            try
            {
                //var elements = uidoc.Selection.PickObjects(ObjectType.Element, "Select Elements");
                IList<Reference> elements = uidoc.Selection.PickObjects(ObjectType.Element, "Select Elements");
                ICollection<ElementId> selectedElementIds = new List<ElementId>();

                foreach (Reference elementRef in elements)
                {
                    ElementId elementId = elementRef.ElementId;
                    selectedElementIds.Add(elementId);
                }
                // Create a FilteredElementCollector to filter elements by category
                FilteredElementCollector collector = new FilteredElementCollector(doc, selectedElementIds);

                structuralFramingElements = collector
                    .OfClass(typeof(FamilyInstance)) // Assuming structural framing elements are FamilyInstances
                    .Where(elem => elem.Category.Name == "Structural Framing");

                revitBeamMapping.ItemsSource = structuralFramingElements;
                // Show the window, use show to make modeless. Not showdialog
                this.Show();

                // Bring the window to the front
                this.Activate();
            }
            catch (Exception)
            {

            }
        }
    }
}
