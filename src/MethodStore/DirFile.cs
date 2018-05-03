using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodStore
{
    internal class DirFile
    {
        private string _baseDirectory;
        private string _pathDataFiles;

        internal DirFile()
        {
            _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            _pathDataFiles = Path.Combine(
                _baseDirectory,
                "Data",
                "Methods");
        }

        private bool CreatePathDataFiles()
        {
            bool dirDataFilesExists = false;

            DirectoryInfo infoPathDataFiles = new DirectoryInfo(_pathDataFiles);
            if (!infoPathDataFiles.Exists)
            {
                try
                {
                    infoPathDataFiles.Create();
                    dirDataFilesExists = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadKey();
                }
            }
            else
                dirDataFilesExists = true;

            return dirDataFilesExists;
        }

        internal List<FileInfo> GetListFilesObjectMethods()
        {
            if (!CreatePathDataFiles())
                return null;

            return new DirectoryInfo(_pathDataFiles).GetFiles().ToList();
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

        private bool Compare(FileInfo a, FileInfo b)
        {
            return (int.Parse(b.Name.Replace(".json", string.Empty)))
                     <
                 (int.Parse(a.Name.Replace(".json", string.Empty)));
        }

        internal void SaveObjectMethods(Guid id, ObjectMethod objectMethod)
        {
            if (!CreatePathDataFiles())
                return;

            new Json().SerializeObjectMethod(new FileInfo(GetFileNameObjectMethod(id)), objectMethod);
        }

        private string GetFileNameObjectMethod(Guid id)
        {
            return Path.Combine(
                _pathDataFiles,
                $"{id.ToString()}.json");
        }

        internal void Delete(string path)
        {
            new FileInfo(path).Delete();
        }
    }
}
