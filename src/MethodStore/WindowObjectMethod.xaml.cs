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

        #region Events

        private RefreshDataGridEvents _refreshDataGrid = new RefreshDataGridEvents();

        #endregion

        #region Fields

        private bool _isNewObject;
        private ObjectMethod _ref;
        private List<TypeMethods> _listTypeMethods = new List<TypeMethods>();
        private List<Parameter> _dataParameters;

        private MainWindow _owner;
        private double _minLeft;
        private double _maxLeft;
        private double _minTop;
        private double _maxTop;

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
                _dataParameters = _ref.Parameters?.ToList();

            if (_dataParameters == null)
                _dataParameters = new List<Parameter>();

            DataContext = _ref;

            _refreshDataGrid.RefreshDataGrid += _refreshDataGrid_RefreshDataGrid;

            ReadFileTypeMethods();
        }

        private void FormObjectMethod_Loaded(object sender, RoutedEventArgs e)
        {
            _owner = (MainWindow)Owner;
            _minLeft = _owner.Left + 10;
            _maxLeft = _owner.Left + _owner.Width - 10 - Width;
            _minTop = _owner.Top + 10;
            _maxTop = _owner.Top + _owner.Height - 10 - Height;

            SetPositionWindow(true);

            TextBoxModule.Focus();

            _refreshDataGrid.EvokeRefreshDataGrid();
        }

        private void FormObjectMethod_Closed(object sender, EventArgs e)
        {
            if (_isNewObject)
                new DirFile().Delete(_ref.Path);
        }

        private void FormObjectMethod_LocationChanged(object sender, EventArgs e)
        {
            SetPositionWindow();
        }

        private void WindowCommandCloseForm_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void WindowCommandSaveObject_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveObjectAndClose();
        }

        #endregion

        #region Button

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            SaveObjectAndClose();
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

            ListCollectionView collectionView = new ListCollectionView(_listTypeMethods);
            collectionView.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));

            ComboBoxTypeMethods.ItemsSource = collectionView;
        }

        private async Task<List<TypeMethods>> ReadFileTypeMethodsAsync()
        {
            return await Task.Run(() => new DirFile().GetListTypeMethods());
        }

        #endregion

        private void SetPositionWindow(bool loaded = false)
        {
            if (Left < _minLeft || loaded)
                Left = _minLeft;
            else if (Left > _maxLeft)
                Left = _maxLeft;

            if (Top < _minTop || loaded)
                Top = _minTop;
            else if (Top > _maxTop)
                Top = _maxTop;
        }

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
            DataGridParameters.CurrentCell = new DataGridCellInfo(newParameter, DataGridParameters.CurrentColumn);
            DataGridParameters.BeginEdit();
        }

        private void SaveObjectAndClose()
        {
            _ref.DateEdited = DateTime.Now;
            _ref.Parameters = _dataParameters.ToArray();
            new UpdateFilesObjectMethod(ID, _ref).Save();
            _isNewObject = false;
            Close();
        }

    }
}
