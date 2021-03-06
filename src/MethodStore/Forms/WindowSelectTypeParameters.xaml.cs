﻿using MahApps.Metro.Controls;
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

        private double _minLeft;
        private double _minTop;

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
            _minLeft = Owner.Left + 10;
            _minTop = Owner.Top + 10;

            SetPositionWindow(true);

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
            Dictionary<string, List<string>> dictionatyTypes = new Files.FileParametersTypes().IninializeDictionatyTypeName();

            foreach (string item in CurrentTypes.Replace(" ", "").Split(','))
            {
                bool findSeparator = item.FirstOrDefault(f => f == '.') != char.MinValue;

                string nameParent = "";
                string nameChildren;

                if (findSeparator)
                {
                    string[] itemSubstring = item.Split('.');
                    nameParent = itemSubstring[0];
                    nameChildren = itemSubstring[1];
                }
                else
                {
                    nameChildren = item;

                    foreach (KeyValuePair<string, List<string>> keyNames in dictionatyTypes)
                    {
                        foreach (string elementNames in keyNames.Value)
                        {
                            if (elementNames == nameChildren)
                            {
                                nameParent = keyNames.Key;
                                break;
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(nameParent))
                {
                    var children = _treeType.Tree.FirstOrDefault(f => f.Text == nameParent)?.Children;

                    if (children != null)
                        for (int i = 0; i < children.Count; i++)
                        {
                            if (children[i].Text == nameChildren)
                                children[i].IsChecked = true;
                        }
                }
            }
        }

        private void SetSelectedTypes()
        {
            List<string> listBaseType = new Files.FileParametersTypes().InitializeListType();

            StringBuilder stringBuilder = new StringBuilder();
            foreach (TreeTypeParameters itemType in _treeType.Tree)
            {
                foreach (TreeTypeParameters itemName in itemType.Children)
                {
                    if (itemName.IsChecked == true)
                    {
                        if (stringBuilder.Length != 0)
                            stringBuilder.Append(", ");

                        if (string.IsNullOrEmpty(listBaseType.FirstOrDefault(f => f == itemType.Text)))
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

        private void SetPositionWindow(bool loaded = false)
        {
            if (Left < _minLeft || loaded)
                Left = _minLeft;

            if (Top < _minTop || loaded)
                Top = _minTop;
        }
    }
}
