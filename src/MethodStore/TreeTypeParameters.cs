using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace MethodStore
{
    public class TreeTypeParameters : INotifyPropertyChanged
    {
        private UpdateSelectedParameterTypesEvents _updateSelectedParameterTypes;

        private ObservableCollection<TreeTypeParameters> _children = new ObservableCollection<TreeTypeParameters>();
        private ObservableCollection<TreeTypeParameters> _parent = new ObservableCollection<TreeTypeParameters>();
        private string _text;
        private string _id;
        private bool? _isChecked = false;
        private bool _isExpanded;
        private string _currentId;

        public ObservableCollection<TreeTypeParameters> Children { get => _children; }
        public ObservableCollection<TreeTypeParameters> Parent { get => _parent; }
        public string Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    _text = value;
                    RaisePropertyChanged("Text");
                }
            }
        }
        public string Id
        {
            get => _id;
            set
            {
                if (_id != value)
                    _id = value;
            }
        }
        public bool? IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    RaisePropertyChanged("IsChecked");
                }
            }
        }
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (_isExpanded != value)
                {
                    _isExpanded = value;
                    RaisePropertyChanged("IsExpanded");
                }
            }
        }

        internal string SelectedTypes { get; private set; }


        internal string CurrentId { set => _currentId = value; }

        internal TreeTypeParameters(UpdateSelectedParameterTypesEvents updateSelectedParameterTypesEvents = null)
        {
            if (updateSelectedParameterTypesEvents != null)
                _updateSelectedParameterTypes = updateSelectedParameterTypesEvents;

            _id = Guid.NewGuid().ToString();
            Tree = new ObservableCollection<TreeTypeParameters>();
        }

        internal ObservableCollection<TreeTypeParameters> Tree { get; private set; }

        internal void SetCurrentID(string currentId)
        {
            CheckBoxId.currentId = currentId;
        }

        internal void CheckParent()
        {
            for (int i = 0; i < Tree.Count; i++)
            {
                CheckParentByChild(Tree[i]);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (propertyName == "IsChecked")
            {
                if (_id == CheckBoxId.currentId)
                {
                    if (_parent.Count == 0 && _children.Count != 0)
                        CheckChildNodes(_children, _isChecked);
                    if (_parent.Count > 0 && _children.Count > 0)
                        CheckChildAndParent(_parent, _children, _isChecked);
                    if (_parent.Count > 0 && _children.Count == 0)
                        CheckParentNodes(_parent);

                    _updateSelectedParameterTypes.EvokeUpdateSelectedParameterTypes();
                }
            }
        }

        private void CheckChildNodes(ObservableCollection<TreeTypeParameters> child, bool? isChecked)
        {
            foreach (TreeTypeParameters item in child)
            {
                item.IsChecked = isChecked;
                if (item.Children.Count > 0)
                    CheckChildNodes(item.Children, isChecked);
            }
        }

        private void CheckParentNodes(ObservableCollection<TreeTypeParameters> parent)
        {
            int count = 0;
            bool isNull = false;

            foreach (TreeTypeParameters itemParent in parent)
            {
                foreach (TreeTypeParameters itemChild in itemParent.Children)
                {
                    count++;
                    if (itemChild.IsChecked == null)
                        isNull = true;
                }

                if (count != itemParent.Children.Count && count > 0)
                    itemParent.IsChecked = null;
                else if (count == 0)
                    itemParent.IsChecked = false;
                else if (count == itemParent.Children.Count && isNull)
                    itemParent.IsChecked = null;
                else if (count == itemParent.Children.Count && !isNull)
                    itemParent.IsChecked = true;

                if (itemParent.Parent.Count != 0)
                    CheckParentNodes(itemParent.Parent);
            }
        }

        private void CheckChildAndParent(ObservableCollection<TreeTypeParameters> parent, ObservableCollection<TreeTypeParameters> child, bool? isChecked)
        {
            CheckChildNodes(child, isChecked);
            CheckParentNodes(parent);
        }

        private void CheckParentByChild(TreeTypeParameters parent)
        {
            bool isChecked = true;
            foreach (TreeTypeParameters item in parent.Children)
            {
                if (!item.IsChecked.HasValue || !item.IsChecked.Value)
                {
                    isChecked = false;
                    break;
                }
            }
            parent.IsChecked = isChecked;
        }

        internal void FillingTree(ParametersTypes parametersTypes)
        {
            Tree.Clear();

            if (parametersTypes == null)
                return;

            foreach (string itemType in parametersTypes.UniqueTypes)
            {
                var levelTypes = new TreeTypeParameters(_updateSelectedParameterTypes) { Text = itemType };

                foreach (string itemName in parametersTypes.DictionaryType[itemType])
                {
                    var levelNames = new TreeTypeParameters(_updateSelectedParameterTypes) { Text = itemName };

                    levelTypes.Children.Add(levelNames);
                    levelNames.Parent.Add(levelTypes);
                }

                Tree.Add(levelTypes);
            }
        }

        private struct CheckBoxId
        {
            public static string currentId = null;
        }

    }
}
