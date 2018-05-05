using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using MahApps.Metro.Controls;

namespace MethodStore
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        #region Private fields

        private bool _formLoaded;

        private RefreshDataGridEvents _refreshDataGrid = new RefreshDataGridEvents();
        private List<ObjectMethod> _dataMethods;

        #endregion

        #region Filter (fields, properties)

        public static readonly DependencyProperty _filterText = DependencyProperty.Register(
            "FilterText",
            typeof(string),
            typeof(MainWindow),
            new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty _filterTypeMethods = DependencyProperty.Register(
            "FilterTypeMethods",
            typeof(bool),
            typeof(MainWindow),
            new UIPropertyMetadata(false));

        public static readonly DependencyProperty _filterModule = DependencyProperty.Register(
            "FilterModule",
            typeof(bool),
            typeof(MainWindow),
            new UIPropertyMetadata(false));

        public static readonly DependencyProperty _filterMethodName = DependencyProperty.Register(
            "FilterMethodName",
            typeof(bool),
            typeof(MainWindow),
            new UIPropertyMetadata(false));

        public static readonly DependencyProperty _filterDescription = DependencyProperty.Register(
            "FilterDescription",
            typeof(bool),
            typeof(MainWindow),
            new UIPropertyMetadata(false));


        private string FilterText
        {
            get { return (string)(GetValue(_filterText)); }
            set
            {
                SetValue(_filterText, value);
                SetItemSourceDataGrid();
            }
        }
        private bool FilterTypeMethods
        {
            get { return (bool)GetValue(_filterTypeMethods); }
            set
            {
                SetValue(_filterTypeMethods, value);
                SetItemSourceDataGrid();
            }
        }
        private bool FilterModule
        {
            get { return (bool)GetValue(_filterModule); }
            set
            {
                SetValue(_filterModule, value);
                SetItemSourceDataGrid();
            }
        }
        private bool FilterMethodName
        {
            get { return (bool)GetValue(_filterMethodName); }
            set
            {
                SetValue(_filterMethodName, value);
                SetItemSourceDataGrid();
            }
        }
        private bool FilterDescription
        {
            get { return (bool)GetValue(_filterDescription); }
            set
            {
                SetValue(_filterDescription, value);
                SetItemSourceDataGrid();
            }
        }

        #endregion

        #region Window event

        public MainWindow()
        {
            InitializeComponent();

            _refreshDataGrid.RefreshDataGrid += _refreshDataGrid_RefreshDataGrid;

            DataContext = this;
        }

        private void MainWindowMethodStore_Loaded(object sender, RoutedEventArgs e)
        {
            FilterTypeMethods = false;
            FilterModule = true;
            FilterMethodName = true;
            FilterDescription = true;

            _formLoaded = true;

            _refreshDataGrid.EvokeRefreshDataGrid();
        }

        private void DataGridDataEnter_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenSelectedObjectMethod();
        }

        #endregion

        #region Button

        private void ButtonAddMethod_Click(object sender, RoutedEventArgs e)
        {
            ShowFormObjectMethod(Guid.NewGuid(), true);
        }

        private void ButtonEditMethod_Click(object sender, RoutedEventArgs e)
        {
            OpenSelectedObjectMethod();
        }

        private void ButtonClearFilter_Click(object sender, RoutedEventArgs e)
        {
            FilterText = string.Empty;
        }

        #endregion

        #region Filter (CheckBox, TextBox)

        private void CheckBoxTypeMethods_Click(object sender, RoutedEventArgs e)
        {
            SetItemSourceDataGrid();
        }

        private void CheckBoxFilterDescription_Click(object sender, RoutedEventArgs e)
        {
            SetItemSourceDataGrid();
        }

        private void CheckBoxFilterMethodName_Click(object sender, RoutedEventArgs e)
        {
            SetItemSourceDataGrid();
        }

        private void CheckBoxFilterModule_Click(object sender, RoutedEventArgs e)
        {
            SetItemSourceDataGrid();
        }

        private void TextBoxFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetItemSourceDataGrid();
        }

        #endregion

        private void SetItemSourceDataGrid()
        {
            if (!_formLoaded)
                return;

            _dataMethods = new UpdateFilesObjectMethod().GetList();

            DataGridData.ItemsSource = GetCollectionFilterItemSource();
        }

        private ICollectionView GetCollectionFilterItemSource()
        {
            string textFilter = TextBoxFilter.Text.ToUpper();

            CollectionViewSource collectionSourceFilter = new CollectionViewSource() { Source = _dataMethods };

            ICollectionView collectionFilter = collectionSourceFilter.View;

            Predicate<object> objectFilter = null;

            if (FilterTypeMethods || FilterModule || FilterMethodName || FilterDescription)
                if (!string.IsNullOrWhiteSpace(textFilter))
                    objectFilter = new Predicate<object>(
                       item => (
                               (FilterTypeMethods && (((ObjectMethod)item).TypeMethodName != null && ((ObjectMethod)item).TypeMethodName.ToUpper().Contains(textFilter)))
                            || (FilterModule && ((ObjectMethod)item).Module.ToUpper().Contains(textFilter))
                            || (FilterMethodName && ((ObjectMethod)item).MethodName.ToUpper().Contains(textFilter))
                            || (FilterDescription && ((ObjectMethod)item).Description.ToUpper().Contains(textFilter))
                            )
                        );

            collectionFilter.Filter = objectFilter;
            return collectionFilter;
        }

        private void _refreshDataGrid_RefreshDataGrid()
        {
            SetItemSourceDataGrid();
        }

        private void ShowFormObjectMethod(Guid id, bool isNewObject = false)
        {
            ObjectMethod objectMethod = DataGridData.SelectedItem as ObjectMethod;

            WindowObjectMethod formObject = new WindowObjectMethod(id, isNewObject)
            {
                Owner = this
            };
            formObject.ShowDialog();
            
            _refreshDataGrid.EvokeRefreshDataGrid();

            //DataGridData.Focus();
            //DataGridData.CurrentCell = new DataGridCellInfo(objectMethod, DataGridData.Columns[0]);
        }

        private void DataGridData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OpenSelectedObjectMethod();
        }

        private void OpenSelectedObjectMethod()
        {
            if (DataGridData.SelectedItem is ObjectMethod objectMethod)
                ShowFormObjectMethod(objectMethod.ID);
        }

        private void MenuItemCopyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridData.SelectedItem is ObjectMethod objectMethod)
                if (!string.IsNullOrWhiteSpace(objectMethod.MethodInvokationString))
                    Clipboard.SetText(objectMethod.MethodInvokationString);
        }

    }
}
