using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace MethodStore
{
    /// <summary>
    /// Логика взаимодействия для WindowSelectTypeParameters.xaml
    /// </summary>
    public partial class WindowSelectTypeParameters : MetroWindow
    {
        internal ParametersTypes ParametersTypes { get; set; }
        internal TreeTypeParameters TreeType { get; private set; }

        internal WindowSelectTypeParameters()
        {
            InitializeComponent();
        }

        private void CheckBoxTree_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeType.SetCurrentID(((CheckBox)sender).Uid);
        }

        private void FormSelectTypeParameters_Loaded(object sender, RoutedEventArgs e)
        {
            TreeType = new TreeTypeParameters();
            TreeType.FillingTree(ParametersTypes);
            TreeViewType.ItemsSource = TreeType.Tree;
        }
    }
}
