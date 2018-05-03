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

        internal ObjectMethod GetFileObjectMethods(int? id = null)
        {
            ObjectMethod refObject = null;

            List<FileInfo> listFiles = GetListFilesObjectMethods();

            if (listFiles == null)
                return refObject;

            int idObject = 1;

            if (listFiles.Count > 0)
            {
                listFiles.Sort((a, b) => string.Compare(b.Name, a.Name));
                idObject = int.Parse(listFiles.First().Name.Replace(".json", string.Empty));
                idObject++;
            }

            string pathObject = Path.Combine(
                _pathDataFiles,
                $"{idObject.ToString()}.json");

            Json json = new Json();

            FileInfo fileObject = new FileInfo(pathObject);
            if (!fileObject.Exists)                                                                 
            {
                json.SerializeObjectMethod(fileObject, new ObjectMethod());
            }

            refObject = json.DeserialiseObjectMethod(fileObject);

            return refObject;
        }

    }
}
