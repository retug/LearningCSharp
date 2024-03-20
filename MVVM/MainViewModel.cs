using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Windows.Input;

namespace etabsRevitCnx
{
    //View Model
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();
        }
    }
    public class MainViewModel : INotifyPropertyChanged
    {
        private UIDocument _uidoc;
        private IEnumerable<RevitFramingModel> _structuralFramingElements;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand GatherBeamsCommand { get; }

        public IEnumerable<RevitFramingModel> StructuralFramingElements
        {
            get { return _structuralFramingElements; }
            set
            {
                _structuralFramingElements = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StructuralFramingElements)));
            }
        }

        public MainViewModel(UIDocument uiDoc)
        {
            _uidoc = uiDoc;
            GatherBeamsCommand = new RelayCommand(GatherBeams);
        }

        public void GatherBeams()
        {
            try
            {
                IList<Reference> elements = _uidoc.Selection.PickObjects(ObjectType.Element, "Select Elements");
                ICollection<ElementId> selectedElementIds = new List<ElementId>();

                foreach (Reference elementRef in elements)
                {
                    ElementId elementId = elementRef.ElementId;
                    selectedElementIds.Add(elementId);
                }

                FilteredElementCollector collector = new FilteredElementCollector(_uidoc.Document, selectedElementIds);

                var structuralFramingElements = collector
                .OfClass(typeof(FamilyInstance))
                    .Where(elem => elem.Category.Name == "Structural Framing")
                    .Select(elem => new RevitFramingModel { Name = elem.Name });

                StructuralFramingElements = structuralFramingElements;
                //revitBeamMapping.ItemsSource = structuralFramingElements;
            }
            catch (Exception)
            {
                // Handle exceptions
            }
        }
    }
}
