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

        public TypeMethods()
        {
        }

        public TypeMethods(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

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
