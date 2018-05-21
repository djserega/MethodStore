using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MethodStore.Files
{
    internal class FileParametersTypes
    {
        internal string FullNameFileTypes { get; }

        public FileParametersTypes()
        {
            FullNameFileTypes = new DirFile().FullNameFileParametersTypes;
        }

        public ParametersTypes ReadFileTypes()
        {
            FileInfo fileInfo = new FileInfo(FullNameFileTypes);
            if (!fileInfo.Exists)
                return null;

            List<string> listType = new List<string>() { "Примитивные типы" };
            Dictionary<string, List<string>> keysType = new Dictionary<string, List<string>>()
            {
                {
                    "Примитивные типы",
                    new List<string>()
                    {
                        "Null",
                        "Неопределено",
                        "Число",
                        "Строка",
                        "Дата",
                        "Булево"
                    }
                }
            };

            Dictionary<string, string> dictionaryTranslate = GetTranslateTypes();
            Dictionary<string, bool> usingType = GetUsingTypes();

            using (XmlReader xmlReader = XmlReader.Create(FullNameFileTypes))
            {
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element
                        && xmlReader.Name == "ChildObjects")
                    {
                        xmlReader.Read();

                        do
                        {
                            string typeName = xmlReader.LocalName;

                            if (dictionaryTranslate.ContainsKey(typeName))
                            {
                                if (usingType.ContainsKey(typeName))
                                {
                                    if (usingType[typeName])
                                    {
                                        string typeNameTranslation = dictionaryTranslate[typeName];

                                        if (!string.IsNullOrWhiteSpace(typeNameTranslation))
                                        {
                                            listType.Add(typeNameTranslation);

                                            xmlReader.Read();

                                            string typeValue = xmlReader.Value;

                                            if (!string.IsNullOrWhiteSpace(typeValue))
                                            {
                                                if (keysType.ContainsKey(typeNameTranslation))
                                                    keysType.First(f => f.Key == typeNameTranslation).Value.Add(typeValue);
                                                else
                                                    keysType.Add(typeNameTranslation, new List<string>() { typeValue });
                                            }
                                        }
                                    }
                                }
                            }
                            xmlReader.Read();
                        }
                        while (!(xmlReader.NodeType == XmlNodeType.EndElement
                            && xmlReader.Name == "ChildObjects"));
                    }
                }
                xmlReader.Close();
            }

            foreach (var item in keysType)
            {
                item.Value.Sort();
            }

            ParametersTypes types = new ParametersTypes()
            {
                UniqueTypes = listType.Distinct().ToList(),
                DictionaryType = keysType
            };

            return types;
        }

        private Dictionary<string, bool> GetUsingTypes()
        {
            Dictionary<string, bool> dictionary = new Dictionary<string, bool>()
            {
                {"PrimitiveTypes", true },
                {"Catalog", true },
                {"Document", true },
                {"Enum", true }
            };
            return dictionary;
        }

        private Dictionary<string, string> GetTranslateTypes()
        {
            Dictionary<string, string> dictionaryTranslate = new Dictionary<string, string>()
            {
                { "PrimitiveTypes", "Примитивные типы" },
                { "Language", "Язык" },
                { "Subsystem", "Подсистемы"},
                { "StyleItem", "Элементы стиля"},
                { "CommonPicture", "Общие картинки"},
                { "Interface", "Интерфейсы"},
                { "SessionParameter", "Параметры сеанса"},
                { "Role", "Роли"},
                { "CommonTemplate", "Общие макеты"},
                { "FilterCriterion", "Критерии отбора"},
                { "CommonModule", "Общие модули"},
                { "CommonAttribute", "Общие реквизиты"},
                { "ExchangePlan", "Планы обмена"},
                { "XDTOPackage", "XDTO-пакеты"},
                { "WebService", "Web-сервисы"},
                { "EventSubscription", "Подписки на события"},
                { "ScheduledJob", "Регламентные задания"},
                { "FunctionalOption", "Функциональные опции"},
                { "FunctionalOptionsParameter", "Параметры функциональных опций"},
                { "CommonCommand", "Общие команды"},
                { "CommandGroup", "Группы команд"},
                { "Constant", "Константы"},
                { "CommonForm", "Общие формы"},
                { "Catalog", "СправочникСсылка"},
                { "Document", "ДокументСсылка"},
                { "DocumentNumerator", "Нумераторы документов"},
                { "Sequence", "Последовательности"},
                { "DocumentJournal", "Журналы документов"},
                { "Enum", "ПеречислениеСсылка"},
                { "Report", "Отчеты"},
                { "DataProcessor", "Обработки"},
                { "InformationRegister", "Регистры сведений"},
                { "AccumulationRegister", "Регистры накопления"},
                { "ChartOfCharacteristicTypes", "Планы видов характеристик"},
                { "BusinessProcess", "Бизнес процессы"},
                { "Task", "Задачи"},
                { "ExternalDataSource", "Внешние источники данных"}
            };
            return dictionaryTranslate;
        }

    }
}
