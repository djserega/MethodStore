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
        private string _baseDirectory;
        private string _pathData;
        private string _pathDataFiles;
        private string _fullNameFileTypeMethods;
        private string _fullNameFileParametersTypes;

        internal string PathData { get => _pathData;  }
        internal string PathDataFiles { get => _pathDataFiles; }
        internal string FullNameFileParametersTypes { get => _fullNameFileParametersTypes; }

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

            _fullNameFileTypeMethods = Path.Combine(
                _pathData,
                "TypeMethods.txt");

            _fullNameFileParametersTypes = Path.Combine(
                _pathData,
                "ParametersTypes.xml");
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
