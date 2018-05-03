using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodStore
{
    public class ObjectMethod
    {
        private string _module;
        private string _methodName;

        public Guid ID { get; set; }
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

        private void SetMethodInvokationString()
        {
            MethodInvokationString = $"{_module}.{_methodName}();";
        }
    }
}
