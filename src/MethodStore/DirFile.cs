using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MethodStore
{
    internal class DirFile
    {
        private string _baseDirectory;
        private string _pathData;
        private string _pathDataFiles;
        private string _fullNameFile;

        internal string PathData { get { return _pathData; } }
        internal string PathDataFiles { get { return _pathDataFiles; } }


        internal DirFile()
        {
            _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            _pathData = Path.Combine(
                _baseDirectory,
                "Data");
            CreateDirectory(_pathData);

            _pathDataFiles = Path.Combine(
                _pathData,
                "Methods");
            CreateDirectory(_pathDataFiles);

            _fullNameFile = Path.Combine(
                _pathData,
                "TypeMethods.txt");
        }

        private void CreateDirectory(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (!directoryInfo.Exists)
                directoryInfo.Create();
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

            List<FileInfo> listFiles = new DirectoryInfo(_pathDataFiles).GetFiles().ToList();
            listFiles.Sort((a, b) => b.LastWriteTimeUtc.CompareTo(a.LastWriteTimeUtc));

            return listFiles;
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

        internal List<TypeMethods> GetListTypeMethods()
        {
            FileInfo fileInfo = new FileInfo(_fullNameFile);
            if (fileInfo.Exists)
                return ReadFilesTypeMethods(fileInfo);
            else
            {
                fileInfo.Create();
                return null;
            }
        }

        private List<TypeMethods> ReadFilesTypeMethods(FileInfo fileInfo)
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

            listTypeMethods.Sort((a, b) => string.Compare(a.Name, b.Name));

            return listTypeMethods;
        }

    }
}
