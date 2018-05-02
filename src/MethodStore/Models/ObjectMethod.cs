using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodStore
{
    public class ObjectMethod
    {
        public string Module { get; set; }
        public string MethodName { get; set; }
        public Parameter[] Parameters { get; set; }
        public string Description { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime DateEdited { get; set; }
        
        public ObjectMethod()
        {
            Module = "";
            MethodName = "";
            Description = "";
            DateCreation = DateTime.Now;
        }
    }
}
