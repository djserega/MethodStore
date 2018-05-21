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
        private UpdateSelectedParameterTypesEvents _updateSelectedParameterTypes = new UpdateSelectedParameterTypesEvents();

        internal string SelectedTypes { get; private set; }
        internal bool PressButton { get; private set; }

        internal string CurrentTypes { get; set; }
        internal ParametersTypes ParametersTypes { get; set; }
        private TreeTypeParameters _treeType;

        internal WindowSelectTypeParameters()
        {
            InitializeComponent();

            _updateSelectedParameterTypes.UpdateSelectedParameterTypes += _updateSelectedParameterTypes_UpdateSelectedParameterTypes;
        }

        private void FormSelectTypeParameters_Loaded(object sender, RoutedEventArgs e)
        {
            _treeType = new TreeTypeParameters(_updateSelectedParameterTypes);
            _treeType.FillingTree(ParametersTypes);

            if (!string.IsNullOrWhiteSpace(CurrentTypes))
            {
                SetCurrentTypes();
                _treeType.CheckParent();
            }

            TreeViewType.ItemsSource = _treeType.Tree;
        }

        private void _updateSelectedParameterTypes_UpdateSelectedParameterTypes()
        {
            SetSelectedTypes();
        }

        private void CheckBoxTree_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _treeType.SetCurrentID(((CheckBox)sender).Uid);
        }

        private void SetCurrentTypes()
        {
            foreach (string item in CurrentTypes.Replace(" ", "").Split(','))
            {
                bool findSeparator = item.FirstOrDefault(f => f == '.') != char.MinValue;

                string nameParent;
                string nameChildren;

                if (findSeparator)
                {
                    string[] itemSubstring = item.Split('.');
                    nameParent = itemSubstring[0];
                    nameChildren = itemSubstring[1];
                }
                else
                {
                    nameParent = "Примитивные типы";
                    nameChildren = item;
                }

                var children = _treeType.Tree.First(f => f.Text == nameParent).Children;

                for (int i = 0; i < children.Count; i++)
                {
                    if (children[i].Text == nameChildren)
                        children[i].IsChecked = true;
                }
            }
        }

        private void SetSelectedTypes()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (TreeTypeParameters itemType in _treeType.Tree)
            {
                foreach (TreeTypeParameters itemName in itemType.Children)
                {
                    if (itemName.IsChecked == true)
                    {
                        if (stringBuilder.Length != 0)
                            stringBuilder.Append(", ");

                        if (itemType.Text != "Примитивные типы")
                        {
                            stringBuilder.Append(itemType.Text);
                            stringBuilder.Append(".");
                        }
                        stringBuilder.Append(itemName.Text);
                    }
                }
            }
            SelectedTypes = stringBuilder.ToString();
            TextBoxSelectedTypes.Text = SelectedTypes;
        }

        private void ButtonSelect_Click(object sender, RoutedEventArgs e)
        {
            PressButton = true;
            Close();
        }
    }
}
