using MethodStore.Files;
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
        private readonly string _baseDirectory;
        private readonly string _fullNameFileTypeMethods;

        internal string PathData { get; }
        internal string PathDataFiles { get; }
        internal string FullNameFileParametersTypes { get; }
        internal string FullNameTextTemplate { get; }

        internal DirFile()
        {
            _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            PathData = Path.Combine(
                _baseDirectory,
                "Data");
            CreateDirectory(PathData);

            PathDataFiles = Path.Combine(
                PathData,
                "Methods");
            CreateDirectory(PathDataFiles);

            _fullNameFileTypeMethods = Path.Combine(
                PathData,
                "TypeMethods.txt");

            FullNameFileParametersTypes = Path.Combine(
                PathData,
                "ParametersTypes.xml");

            FullNameTextTemplate = Path.Combine(
                PathData,
                "MethodStore.st");
        }

        private void CreateDirectory(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (!directoryInfo.Exists)
                directoryInfo.Create();
        }

        internal void Delete(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
                fileInfo.Delete();
        }

        internal List<TypeMethods> GetListTypeMethods()
        {
            FileInfo fileInfo = new FileInfo(_fullNameFileTypeMethods);
            if (fileInfo.Exists)
                return new FileTypeMethods().ReadFilesTypeMethods(fileInfo);
            else
            {
                fileInfo.Create();
                return null;
            }
        }
    }
}
