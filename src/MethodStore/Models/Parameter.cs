using System.ComponentModel;

namespace MethodStore
{
    public class Parameter
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string ValueByDefault { get; set; }
        public bool ByValue { get; set; }

        public Parameter()
        {
        }

        public Parameter(string name)
        {
            Name = name;
        }
    }
}
