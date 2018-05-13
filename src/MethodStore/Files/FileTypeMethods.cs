using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodStore.Files
{
    internal class FileTypeMethods
    {
        internal List<TypeMethods> ReadFilesTypeMethods(FileInfo fileInfo)
        {
            List<TypeMethods> listTypeMethods = new List<TypeMethods>();

            using (StreamReader reader = new StreamReader(fileInfo.FullName, Encoding.Default))
            {
                while (!reader.EndOfStream)
                {
                    string textInLine = reader.ReadLine();

                    string groupName = "<Без категории>";
                    string typeName = string.Empty;

                    int indexSemicolon = textInLine.IndexOf(';');
                    if (indexSemicolon == -1)
                        typeName = textInLine;
                    else
                    {
                        groupName = textInLine.Substring(0, indexSemicolon);
                        typeName = textInLine.Substring(indexSemicolon + 1);
                    }
                    listTypeMethods.Add(new TypeMethods(typeName, groupName));
                }
            }

            listTypeMethods.Sort((a, b) => string.Compare(a.GroupName + a.Name, b.GroupName + b.Name));

            return listTypeMethods;
        }
    }
}
