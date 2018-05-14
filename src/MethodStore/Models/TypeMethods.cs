using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodStore
{
    public class TypeMethods
    {
        private string _name;
        private string _groupName;

        public string Name
        {
            get { return _name; }
            set
            {
                string tempValue = value.Trim();
                if (_name != tempValue)
                    _name = tempValue;
            }
        }
        public string GroupName
        {
            get { return _groupName; }
            set
            {
                string tempValue = value?.Trim();
                if (_groupName != tempValue)
                    _groupName = tempValue;
            }
        }


        public TypeMethods()
        {
        }

        public TypeMethods(string name, string groupName = "")
        {
            Name = name;
            GroupName = groupName;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is TypeMethods))
                return false;

            return ((TypeMethods)obj).Name == Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
