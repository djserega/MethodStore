using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodStore
{
    public class TypeMethods
    {
        public string Name { get; set; }
        public string GroupName { get; set; }

        public TypeMethods()
        {
        }

        public TypeMethods(string name, string groupName = "")
        {
            Name = name;
            GroupName = groupName;
        }

        //public override string ToString()
        //{
        //    if (!string.IsNullOrWhiteSpace(GroupName))
        //        return GroupName;
        //    else
        //        return Name;
        //}

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is TypeMethods))
                return false;

            return ((TypeMethods)obj).Name == this.Name;
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
    }
}
