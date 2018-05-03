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
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace MethodStore
{
    /// <summary>
    /// Логика взаимодействия для WindowObjectMethod.xaml
    /// </summary>
    public partial class WindowObjectMethod : MetroWindow
    {
        internal int ID { get; private set; }
        private ObjectMethod _ref;

        public WindowObjectMethod(int? id = null)
        {
            InitializeComponent();

            _ref = new UpdateFilesObjectMethod(id).GetObjectMethod();

            ID = _ref.ID;

            DataContext = _ref;
                                       
            Title += " (новый)";
        }

        private void ButtobAddParameter_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonDeleteParameter_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
