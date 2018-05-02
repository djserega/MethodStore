using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private RefreshDataGridEvents _refreshDataGrid;

        private ICollection<ObjectMethod> _dataMethods;

        public MainWindow()
        {
            InitializeComponent();

            _refreshDataGrid = new RefreshDataGridEvents();
            _dataMethods = new ObservableCollection<ObjectMethod>();

            _refreshDataGrid.RefreshDataGrid += _refreshDataGrid_RefreshDataGrid;
        }

        private void MainWindowMethodStore_Loaded(object sender, RoutedEventArgs e)
        {
            _refreshDataGrid.EvokeRefreshDataGrid();
        }


        private void ButtonAddMethod_Click(object sender, RoutedEventArgs e)
        {
            _dataMethods.Add(new ObjectMethod()
            {
                Description = "new object " + DateTime.Now.ToString(),
                DateEdited = DateTime.Now
            });

            WindowObjectMethod formObject = new WindowObjectMethod();
            formObject.ShowDialog();

            _refreshDataGrid.EvokeRefreshDataGrid();
        }

        private void ButtonEditMethod_Click(object sender, RoutedEventArgs e)
        {

        }

        private void _refreshDataGrid_RefreshDataGrid()
        {
            SetItemSourceDataGrid();
        }

        private void SetItemSourceDataGrid()
        {
            DataGridData.ItemsSource = _dataMethods;
        }
    }
}
