using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodStore.Files
{
    internal class FileObjectMethods
    {
        internal string PathListFiles { get; }

        public FileObjectMethods()
        {
            PathListFiles = new DirFile().PathDataFiles;
        }

        public FileObjectMethods(string pathListFiles)
        {
            PathListFiles = pathListFiles;
        }

        internal List<FileInfo> GetListFilesObjectMethods()
        {
            if (!CreatePathDataFiles())
                return null;

            List<FileInfo> listFiles = new DirectoryInfo(PathListFiles).GetFiles("*.json").ToList();
            listFiles.Sort((a, b) => b.LastWriteTimeUtc.CompareTo(a.LastWriteTimeUtc));

            return listFiles;
        }
        
        internal void SaveObjectMethods(Guid id, ObjectMethod objectMethod)
        {
            if (!CreatePathDataFiles())
                return;

            new Json().SerializeObjectMethod(new FileInfo(GetFileNameObjectMethod(id)), objectMethod);
        }

        internal ObjectMethod GetFileObjectMethods(Guid id)
        {
            ObjectMethod refObject = null;

            List<FileInfo> listFiles = GetListFilesObjectMethods();

            if (listFiles == null)
                return refObject;

            Json json = new Json();

            FileInfo fileObject = new FileInfo(GetFileNameObjectMethod(id));
            if (!fileObject.Exists)
            {
                json.SerializeObjectMethod(fileObject, new ObjectMethod());
            }

            refObject = json.DeserialiseObjectMethod(fileObject);
            refObject.ID = id;
            refObject.Path = fileObject.FullName;

            return refObject;
        }

        private string GetFileNameObjectMethod(Guid id)
        {
            return Path.Combine(
                PathListFiles,
                $"{id.ToString()}.json");
        }
        
        private bool CreatePathDataFiles()
        {
            bool dirDataFilesExists = false;

            DirectoryInfo infoPathDataFiles = new DirectoryInfo(PathListFiles);
            if (!infoPathDataFiles.Exists)
            {
                try
                {
                    infoPathDataFiles.Create();
                    dirDataFilesExists = true;
                }
                catch (Exception)
                {
                }
            }
            else
                dirDataFilesExists = true;

            return dirDataFilesExists;
        }
    }
}
