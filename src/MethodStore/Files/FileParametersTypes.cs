using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MethodStore.Files
{
    internal class FileParametersTypes
    {
        internal string FullNameFileTypes { get; }

        public FileParametersTypes()
        {
            FullNameFileTypes = new DirFile().FullNameFileParametersTypes;
        }

        public ParametersTypes ReadFileTypes()
        {
            FileInfo fileInfo = new FileInfo(FullNameFileTypes);
            if (!fileInfo.Exists)
                return null;

            List<string> listType = new List<string>();
            Dictionary<string, List<string>> keysType = new Dictionary<string, List<string>>();

            using (XmlReader xmlReader = XmlReader.Create(FullNameFileTypes))
            {
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element
                        && xmlReader.Name == "ChildObjects")
                    {
                        xmlReader.Read();

                        do
                        {
                            string typeName = xmlReader.LocalName;

                            if (!string.IsNullOrWhiteSpace(typeName))
                            {
                                listType.Add(typeName);

                                xmlReader.Read();

                                string typeValue = xmlReader.Value;

                                if (!string.IsNullOrWhiteSpace(typeValue))
                                {
                                    if (keysType.ContainsKey(typeName))
                                        keysType.First(f => f.Key == typeName).Value.Add(typeValue);
                                    else
                                        keysType.Add(typeName, new List<string>() { typeValue });
                                }
                            }
                            xmlReader.Read();
                        }
                        while (!(xmlReader.NodeType == XmlNodeType.EndElement
                            && xmlReader.Name == "ChildObjects"));
                    }
                }
                xmlReader.Close();
            }

            ParametersTypes types = new ParametersTypes()
            {
                UniqueTypes = listType.Distinct().ToList(),
                DictionaryType = keysType
            };

            return types;
        }
    }
}
