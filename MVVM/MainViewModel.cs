using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;

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

    public class StructuralFramingUpdater : IUpdater
    {
        static AddInId m_appId;
        static UpdaterId m_updaterId;
        private readonly MainViewModel _mainViewModel;

        public StructuralFramingUpdater(AddInId id, MainViewModel mainViewModel)
        {
            m_appId = id;
            m_updaterId = new UpdaterId(m_appId, new Guid("FBFBF6B2-4C06-42d4-97C1-D1B4EB593EFF"));
            _mainViewModel = mainViewModel;
        }

        public void Execute(UpdaterData data)
        {
            ObservableCollection<RevitFramingModel> structuralFramingElements = _mainViewModel.StructuralFramingElements;
            // Check if the modified element is a structural framing element
            foreach (ElementId elementId in data.GetModifiedElementIds())
            {
                Element element = data.GetDocument().GetElement(elementId);
                if (element != null && element.Category.Name == "Structural Framing")
                {
                    // Update the corresponding RevitFramingModel
                    RevitFramingModel framingModel = structuralFramingElements.FirstOrDefault(m => m.Id == element.UniqueId);
                    if (framingModel != null)
                    {
                        framingModel.Name = element.Name;
                    }
                }
            }
            _mainViewModel.StructuralFramingElements = structuralFramingElements;
        }

        public string GetAdditionalInformation()
        {
            return "Structural Framing Updater: updates structural framing models when changes occur";
        }

        public ChangePriority GetChangePriority()
        {
            return ChangePriority.Annotations;
        }

        public UpdaterId GetUpdaterId()
        {
            return m_updaterId;
        }

        public string GetUpdaterName()
        {
            return "Structural Framing Updater";
        }
    }


    public class MainViewModel : INotifyPropertyChanged
    {
        private UIDocument _uidoc;
        //private IEnumerable<RevitFramingModel> _structuralFramingElements;
        private ObservableCollection<RevitFramingModel> _structuralFramingElements = new ObservableCollection<RevitFramingModel>();

        public event PropertyChangedEventHandler PropertyChanged;
        

        public ICommand GatherBeamsCommand { get; }

        //public IEnumerable<RevitFramingModel> StructuralFramingElements
        //{
        //    get { return _structuralFramingElements; }
        //    set
        //    {
        //        _structuralFramingElements = value;
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StructuralFramingElements)));
        //    }
        //}
        public ObservableCollection<RevitFramingModel> StructuralFramingElements
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


            StructuralFramingUpdater updater = new StructuralFramingUpdater(_uidoc.Application.ActiveAddInId, this);
            UpdaterRegistry.RegisterUpdater(updater);
            UpdaterRegistry.AddTrigger(updater.GetUpdaterId(), new ElementClassFilter(typeof(FamilyInstance)), Element.GetChangeTypeAny());
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
                    .Select(elem => new RevitFramingModel
                    {
                        Name = elem.Name,
                        Id = elem.UniqueId // Assigning UniqueId as the Id
                    });

                StructuralFramingElements = new ObservableCollection<RevitFramingModel>(structuralFramingElements);
                //revitBeamMapping.ItemsSource = structuralFramingElements;
            }
            catch (Exception)
            {
                // Handle exceptions
            }
        }
    }
}
