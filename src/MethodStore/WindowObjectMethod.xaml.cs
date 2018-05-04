using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace MethodStore
{
    /// <summary>
    /// Логика взаимодействия для WindowObjectMethod.xaml
    /// </summary>
    public partial class WindowObjectMethod : MetroWindow
    {
        internal Guid ID { get; private set; }

        #region Fields

        private RefreshDataGridEvents _refreshDataGrid = new RefreshDataGridEvents();

        private bool _isNewObject;
        private ObjectMethod _ref;
        private List<TypeMethods> _listTypeMethods = new List<TypeMethods>();
        private List<Parameter> _dataParameters = new List<Parameter>();

        #endregion

        #region Event window

        public WindowObjectMethod(Guid id, bool isNewObject = false)
        {
            _isNewObject = isNewObject;

            InitializeComponent();

            _ref = new UpdateFilesObjectMethod(id).Get();

            ID = _ref.ID;

            if (_isNewObject)
                GetTextInClipboard();
            else
                _dataParameters = _ref.Parameters.ToList();

            DataContext = _ref;
            
            _refreshDataGrid.RefreshDataGrid += _refreshDataGrid_RefreshDataGrid;

            ReadFileTypeMethods();
        }

        private void FormObjectMethod_Loaded(object sender, RoutedEventArgs e)
        {
            _refreshDataGrid.EvokeRefreshDataGrid();
        }

        private void FormObjectMethod_Closed(object sender, EventArgs e)
        {
            if (_isNewObject)
                new DirFile().Delete(_ref.Path);
        }

        private void WindowCommandCloseForm_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        #endregion

        #region Button

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            _ref.DateEdited = DateTime.Now;
            _ref.Parameters = _dataParameters.ToArray();
            new UpdateFilesObjectMethod(ID, _ref).Save();
            _isNewObject = false;
            Close();
        }

        private void ButtobAddParameter_Click(object sender, RoutedEventArgs e)
        {
            Parameter newParameter = new Parameter();

            _dataParameters.Add(newParameter);
            SetItemSourceDataGridParameters();
            SetSelectedDataGridParameters(newParameter, "ParametersColumnName");
        }


        private void ButtonDeleteParameter_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridParameters.SelectedItem is Parameter parameter)
            {
                _dataParameters.Remove(parameter);
                SetItemSourceDataGridParameters();
            }
        }

        private void ButtonCopyToClipBoard_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(_ref.MethodInvokationString);
        }

        #endregion

        #region Event element

        private void ComboBoxTypeMethods_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
                if (comboBox.SelectedItem is TypeMethods typeMethods)
                    _ref.TypeMethods = typeMethods;
        }

        private void TextBoxModule_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBoxMethodInvokationString.Text = _ref.MethodInvokationString;
        }

        private void TextBoxMethodName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBoxMethodInvokationString.Text = _ref.MethodInvokationString;
        }

        #endregion

        #region Clipboard

        private void GetTextInClipboard()
        {
            string textClipboard = Clipboard.GetText();

            if (string.IsNullOrWhiteSpace(textClipboard))
                return;

            int positionDot = textClipboard.IndexOf('.');

            if (positionDot > 0)
            {
                _ref.Module = new string(textClipboard.Take(positionDot).ToArray());

                string tempModule = _ref.Module + ".";
                textClipboard = textClipboard.TrimStart(tempModule.ToCharArray());

                int positionBracket = textClipboard.IndexOf("(");
                if (positionBracket > 0)
                    _ref.MethodName = new string(textClipboard.Take(positionBracket).ToArray());
                else
                    _ref.MethodName = textClipboard;
                StackPanelClipboard.Visibility = Visibility.Collapsed;
            }
            else
            {
                TextBoxClipboard.Text = textClipboard;
                StackPanelClipboard.Visibility = Visibility.Visible;
            }
        }

        #endregion

        #region Type methods

        private async void ReadFileTypeMethods()
        {
            _listTypeMethods = await ReadFileTypeMethodsAsync();
            ComboBoxTypeMethods.ItemsSource = _listTypeMethods;
        }

        private async Task<List<TypeMethods>> ReadFileTypeMethodsAsync()
        {
            return await Task.Run(() => new DirFile().GetListTypeMethods());
        }

        #endregion

        private void _refreshDataGrid_RefreshDataGrid()
        {
            SetItemSourceDataGridParameters();
        }

        private void SetItemSourceDataGridParameters()
        {
            CollectionViewSource collectionSource = new CollectionViewSource() { Source = _dataParameters };
            DataGridParameters.ItemsSource = collectionSource.View;
        }

        private void SetSelectedDataGridParameters(Parameter newParameter, string columnName)
        {
            DataGridParameters.SelectedItem = newParameter;

            DataGridParameters.Focus();
            DataGridParameters.CurrentColumn = (DataGridColumn)FormObjectMethod.FindName(columnName);
            DataGridParameters.BeginEdit();
        }

    }
}
