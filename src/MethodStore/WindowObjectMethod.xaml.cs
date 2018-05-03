﻿using System;
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
        internal Guid ID { get; private set; }
        private ObjectMethod _ref;
        private bool _isNewObject;

        public WindowObjectMethod(Guid id, bool isNewObject = false)
        {
            _isNewObject = isNewObject;

            InitializeComponent();

            _ref = new UpdateFilesObjectMethod(id).Get();

            ID = _ref.ID;

            if (_isNewObject)
                GetTextInClipboard();

            DataContext = _ref;
        }

        private void ButtobAddParameter_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonDeleteParameter_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            _ref.DateEdited = DateTime.Now;
            new UpdateFilesObjectMethod(ID, _ref).Save();
            _isNewObject = false;
            Close();
        }

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
            }
            else
                TextBoxClipBoard.Text = textClipboard;
        }

        private void FormObjectMethod_Closed(object sender, EventArgs e)
        {
            if (_isNewObject)
                new DirFile().Delete(_ref.Path);
        }
    }
}
