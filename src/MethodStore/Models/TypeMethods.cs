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
    }
}
