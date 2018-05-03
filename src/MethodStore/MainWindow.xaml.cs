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

        private RefreshDataGridEvents _refreshDataGrid;
        private List<ObjectMethod> _dataMethods;

        #endregion

        #region Filter (fields, properties)

        public static readonly DependencyProperty _filterText = DependencyProperty.Register(
            "FilterText",
            typeof(string),
            typeof(MainWindow),
            new UIPropertyMetadata(string.Empty));

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

            _refreshDataGrid = new RefreshDataGridEvents();
            _refreshDataGrid.RefreshDataGrid += _refreshDataGrid_RefreshDataGrid;

            DataContext = this;
        }

        private void MainWindowMethodStore_Loaded(object sender, RoutedEventArgs e)
        {
            FilterModule = true;
            FilterMethodName = true;
            FilterDescription = true;

            _formLoaded = true;

            _refreshDataGrid.EvokeRefreshDataGrid();
        }

        #endregion

        #region Button

        private void ButtonAddMethod_Click(object sender, RoutedEventArgs e)
        {
            ShowFormObjectMethod(Guid.NewGuid(), true);
        }

        private void ButtonEditMethod_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridData.SelectedItem is ObjectMethod objectMethod)
                ShowFormObjectMethod(objectMethod.ID);
        }

        #endregion

        #region Checkbox Filter

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

        #endregion

        private void TextBoxFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetItemSourceDataGrid();
        }

        private void SetItemSourceDataGrid()
        {
            if (!_formLoaded)
                return;

            _dataMethods = new UpdateFilesObjectMethod().GetList();

            string textFilter = TextBoxFilter.Text.ToUpper();

            CollectionViewSource collectionSourceFilter = new CollectionViewSource() { Source = _dataMethods };

            ICollectionView collectionFilter = collectionSourceFilter.View;

            Predicate<object> objectFilter = null;

            if (FilterModule || FilterMethodName || FilterDescription)
                if (!string.IsNullOrWhiteSpace(textFilter))
                    objectFilter = new Predicate<object>(
                       item => (
                                (FilterModule && ((ObjectMethod)item).Module.ToUpper().Contains(textFilter))
                            ||  (FilterMethodName && ((ObjectMethod)item).MethodName.ToUpper().Contains(textFilter))
                            ||  (FilterDescription && ((ObjectMethod)item).Description.ToUpper().Contains(textFilter))
                            )
                        );

            collectionFilter.Filter = objectFilter;

            DataGridData.ItemsSource = collectionFilter;
        }

        private void _refreshDataGrid_RefreshDataGrid()
        {
            SetItemSourceDataGrid();
        }

        private void ButtonClearFilter_Click(object sender, RoutedEventArgs e)
        {
            FilterText = string.Empty;
        }

        private void ShowFormObjectMethod(Guid id, bool isNewObject = false)
        {
            WindowObjectMethod formObject = new WindowObjectMethod(id, isNewObject)
            {
                Owner = this
            };
            formObject.ShowDialog();

            _refreshDataGrid.EvokeRefreshDataGrid();
        }
    }
}
