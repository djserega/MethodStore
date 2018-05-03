using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodStore
{
    internal class UpdateFilesObjectMethod
    {
        internal int? Id { get; private set; }
        internal ObjectMethod RefObjectMethod { get; private set; }

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
    }
}
