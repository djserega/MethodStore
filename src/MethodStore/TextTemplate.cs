using MethodStore.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodStore
{
    internal class TextTemplate
    {
        private readonly string _fullNameFileTemplate = new DirFile().FullNameTextTemplate;

        internal void UpdateFile()
        {
            try
            {
                UpdateFileTemplate(new FileInfo(_fullNameFileTemplate));
                Dialog.ShowMessage("Файл шаблонов обновлен.");
            }
            catch (Exception ex)
            {
                Dialog.ShowMessage($"Ошибка обновления файла шаблона.\n" +
                    $"{ex.Message}");
            }
        }

        private void UpdateFileTemplate(FileInfo fileInfo)
        {
            List<ObjectMethod> list = new UpdateFilesObjectMethod().GetList().FindAll(f => f.AddToTextTemplate);

            using (StreamWriter streamWriter = new StreamWriter(_fullNameFileTemplate))
            {
                streamWriter.WriteLine("{1,");

                streamWriter.WriteLine($"{{{list.Count},");

                streamWriter.WriteLine("{\"MethodStore\",1,0,\"\", \"\"},");

                StringBuilder stringBuilder = new StringBuilder();
                int i = 0;
                foreach (ObjectMethod item in list)
                {
                    i++;

                    stringBuilder.AppendLine($"{{0,");
                    stringBuilder.AppendLine($"{{\"{item.Name}\"," +
                        $"0," +
                        $"{Convert.ToInt16(item.AddToContextMenu)}," +
                        $"\"{item.TextAutoCorrect}\"," +
                        $"\"{item.MethodInvokationString}\"}}");
                    stringBuilder.Append($"}}");
                    if (list.Count != i)
                        stringBuilder.Append(",");

                    streamWriter.WriteLine(stringBuilder.ToString());

                    stringBuilder.Clear();
                }

                streamWriter.WriteLine("}");

                streamWriter.WriteLine("}");

                streamWriter.Flush();
            }
        }

    }

    internal class ObjectTemplate
    {
        internal string Name { get; set; }
        internal string TextAutoCorrect { get; set; }
        internal bool AddToContextMenu { get; set; }
        internal string Text { get; set; }
    }
}
