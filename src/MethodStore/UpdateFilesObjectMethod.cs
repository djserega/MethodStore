using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodStore
{
    internal class UpdateFilesObjectMethod
    {
        internal int? Id { get; private set; }
        internal ObjectMethod RefObjectMethod { get; private set; }

        internal UpdateFilesObjectMethod()
        {
        }
        internal UpdateFilesObjectMethod(int? id)
        {
            Id = id;
        }

        internal ObjectMethod GetObjectMethod()
        {
            RefObjectMethod = new DirFile().GetFileObjectMethods(Id);

            Id = RefObjectMethod.ID;

            return RefObjectMethod;
        }

        internal List<ObjectMethod> GetListObjectMethod()
        {
            Json json = new Json();

            List<ObjectMethod> listObject = new List<ObjectMethod>();

            foreach (FileInfo item in new DirFile().GetListFilesObjectMethods())
            {
                listObject.Add(json.DeserialiseObjectMethod(item));
            }

            return listObject;
        }
    }
}
