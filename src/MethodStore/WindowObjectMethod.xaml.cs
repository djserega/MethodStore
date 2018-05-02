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
        private ObjectMethod _ref;

        public WindowObjectMethod()
        {
            InitializeComponent();

            _ref = new ObjectMethod()
            {
                DateCreation = DateTime.Now,
                Description = "Установка активного элемента на первом не заполненном реквизите",
                MethodName = "УстановитьТекущийЭлементФормы",
                Module = "ParfumsОбщегоНазначенияКлиентСервер",
                DateEdited = DateTime.Now
            };

            DataContext = _ref;
                                       
            Title += " (новый)";
        }
    }
}
