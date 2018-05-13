using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodStore
{
    internal class ParametersTypes
    {
        internal List<string> UniqueTypes { get; set; }
        internal IDictionary<string, List<string>> DictionaryType { get; set; }

        internal List<string> GetListTypeByTypeName(string type)
        {
            if (!DictionaryType.ContainsKey(type))
                return null;

            return DictionaryType.First(f => f.Key == type).Value;
        }
    }
}
