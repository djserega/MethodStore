using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodStore
{
    [Serializable]
    public class ObjectMethod
    {
        private string _module;
        private string _methodName;

        public Guid ID { get; set; }
        public TypeMethods TypeMethods { get; set; }
        public string TypeMethodName { get { return TypeMethods?.Name; } }
        public string Module
        {
            get { return _module; }
            set
            {
                if (_module != value)
                {
                    _module = value;
                    SetMethodInvokationString();
                }
            }
        }
        public string MethodName
        {
            get { return _methodName; }
            set
            {
                if (_methodName != value)
                {
                    _methodName = value;
                    SetMethodInvokationString();
                }
            }
        }
        public string MethodInvokationString { get; set; }
        public Parameter[] Parameters { get; set; }
        public string Description { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime DateEdited { get; set; }

        public string Path { get; set; }
        
        public ObjectMethod()
        {
            Module = "";
            MethodName = "";
            Description = "";
            DateCreation = DateTime.Now;
        }

        internal void DeleteObject()
        {
            if (!string.IsNullOrWhiteSpace(Path))
                new DirFile().Delete(Path);
        }

        private void SetMethodInvokationString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 1; i < Parameters?.Count(); i++)
                stringBuilder.Append(',');

            string textParameters = stringBuilder.ToString();

            MethodInvokationString = $"{_module}.{_methodName}({textParameters});";
        }
    }
}
