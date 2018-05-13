using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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
using MethodStore.Files;

namespace MethodStore
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        #region Private fields

        private bool _isetSourceInitialized;

        private RefreshDataGridEvents _refreshDataGrid = new RefreshDataGridEvents();
        private CallUpdateListObjectMethodsEvents _callUpdate = new CallUpdateListObjectMethodsEvents();
        private GlobalHotKeyEvents _globalHotKeyEvents = new GlobalHotKeyEvents();

        private SubscriberWatcher _subscriberWatcher;

        private List<ObjectMethod> _dataMethods;
        private ObjectMethod _selectedObjectMethod;

        private GlobalHotKeyManager _globalHotKeyManager;

        private ParametersTypes _parametersTypes;

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
            _callUpdate.CallUpdateListObjectMethods += _callUpdate_CallUpdateListObjectMethods;
            _globalHotKeyEvents.GlobalHotKeyOpenFormObjectEvent += _globalHotKeyEvents_GlobalHotKeyOpenFormObjectEvent;
            _globalHotKeyEvents.GlobalHotKeyMainMenuEvent += _globalHotKeyEvents_GlobalHotKeyMainMenuEvent; ;

            _subscriberWatcher = new SubscriberWatcher(_callUpdate);
            _globalHotKeyManager = new GlobalHotKeyManager(_globalHotKeyEvents);

            DataContext = this;
        }

        private void MainWindowMethodStore_Loaded(object sender, RoutedEventArgs e)
        {
            FilterTypeMethods = false;
            FilterModule = true;
            FilterMethodName = true;
            FilterDescription = true;

            _isetSourceInitialized = true;

            _refreshDataGrid.EvokeRefreshDataGrid();

            ReadFileTypeParameters();
        }

        private void MainWindowMethodStore_Closed(object sender, EventArgs e)
        {
            _globalHotKeyManager.Dispose();
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

        private void ButtonDeleteMethod_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedObjectMethod != null)
            {
                _selectedObjectMethod.DeleteObject();
                SetItemSourceDataGrid();
            }
        }

        private void ButtonClearFilter_Click(object sender, RoutedEventArgs e)
        {
            FilterText = string.Empty;
        }

        private void ButtonUncheckedFilter_Click(object sender, RoutedEventArgs e)
        {
            _isetSourceInitialized = false;

            FilterTypeMethods = false;
            FilterModule = false;
            FilterMethodName = false;
            FilterDescription = false;

            _isetSourceInitialized = true;

            SetItemSourceDataGrid();
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

        #region DataGrid

        private void SetItemSourceDataGrid()
        {
            if (!_isetSourceInitialized)
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

        private void DataGridDataEnter_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenSelectedObjectMethod();
        }

        private void DataGridData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedObjectMethod = DataGridData.SelectedItem as ObjectMethod;
        }

        private void DataGridData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OpenSelectedObjectMethod();
        }

        #endregion

        #region Events

        private void _refreshDataGrid_RefreshDataGrid()
        {
            SetItemSourceDataGrid();
        }

        private void _callUpdate_CallUpdateListObjectMethods(bool NeedNotified)
        {
            Dispatcher.Invoke(new ThreadStart(delegate
            {
                try
                {
                    SetItemSourceDataGrid();
                }
                catch (Exception)
                {
                }
            }));
        }

        private void _globalHotKeyEvents_GlobalHotKeyOpenFormObjectEvent()
        {
            Process currentProcess = Process.GetCurrentProcess();
            IntPtr hWnd = currentProcess.MainWindowHandle;
            if (hWnd != IntPtr.Zero)
            {
                SetForegroundWindow(hWnd);
                ShowWindow(hWnd, 3);
                ShowFormObjectMethod(Guid.NewGuid(), true);
            }
        }

        private void _globalHotKeyEvents_GlobalHotKeyMainMenuEvent()
        {
            Process currentProcess = Process.GetCurrentProcess();
            IntPtr hWnd = currentProcess.MainWindowHandle;
            if (hWnd != IntPtr.Zero)
            {
                SetForegroundWindow(hWnd);
                ShowWindow(hWnd, 3);
            }
        }

        [DllImport("user32.dll")]
        internal static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        #endregion

        #region Parameters types

        private async void ReadFileTypeParameters()
        {
            _parametersTypes = await ReadFileTypeParametersAsync();
        }

        private async Task<ParametersTypes> ReadFileTypeParametersAsync()
        {
            return await Task.Run(() => new FileParametersTypes().ReadFileTypes());
        }

        #endregion

        private void OpenSelectedObjectMethod()
        {
            if (DataGridData.SelectedItem is ObjectMethod objectMethod)
                ShowFormObjectMethod(objectMethod.ID);
        }

        private void ShowFormObjectMethod(Guid id, bool isNewObject = false)
        {
            WindowObjectMethod formObject = new WindowObjectMethod(id, isNewObject)
            {
                Owner = this,
                ParametersTypes = _parametersTypes
            };
            formObject.ShowDialog();
        }

        private void MenuItemCopyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridData.SelectedItem is ObjectMethod objectMethod)
                if (!string.IsNullOrWhiteSpace(objectMethod.MethodInvokationString))
                    Clipboard.SetText(objectMethod.MethodInvokationString);
        }
    }
}
