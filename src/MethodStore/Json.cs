using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace MethodStore
{
    internal class Json
    {
        internal ObjectMethod DeserialiseObjectMethod(FileInfo fileInfo)
        {
            string data = string.Empty;
            using (StreamReader reader = new StreamReader(fileInfo.FullName))
            {
                data = reader.ReadToEnd();
            };

            return Deserialize(data);
        }

        internal void SerializeObjectMethod(FileInfo fileInfo, ObjectMethod objectMethod)
        {
            using (StreamWriter writer = new StreamWriter(fileInfo.FullName))
            {
                writer.WriteLine(Serialize(objectMethod));
                writer.Flush();
            }
        }

        private ObjectMethod Deserialize(string inputText)
        {
            var serializer = new JavaScriptSerializer
            {
                MaxJsonLength = inputText.Length
            };

            return serializer.Deserialize<ObjectMethod>(inputText);
        }

        private string Serialize(ObjectMethod obj)
        {
            var serializer = new JavaScriptSerializer();

            return serializer.Serialize(obj);
        }
    }
}
