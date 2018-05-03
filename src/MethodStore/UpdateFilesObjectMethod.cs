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
        internal Guid Id { get; private set; }
        internal ObjectMethod RefObjectMethod { get; private set; }

        internal UpdateFilesObjectMethod()
        {
        }
        internal UpdateFilesObjectMethod(Guid id)
        {
            Id = id;
        }
        internal UpdateFilesObjectMethod(Guid id, ObjectMethod objectMethod)
        {
            Id = id;
            RefObjectMethod = objectMethod;
        }

        internal ObjectMethod Get()
        {
            RefObjectMethod = new DirFile().GetFileObjectMethods(Id);

            Id = RefObjectMethod.ID;

            return RefObjectMethod;
        }

        internal List<ObjectMethod> GetList()
        {
            Json json = new Json();

            List<ObjectMethod> listObject = new List<ObjectMethod>();

            foreach (FileInfo item in new DirFile().GetListFilesObjectMethods())
            {
                listObject.Add(json.DeserialiseObjectMethod(item));
            }

            return listObject;
        }

        internal void Save()
        {
            new DirFile().SaveObjectMethods(Id, RefObjectMethod);
        }

    }
}
