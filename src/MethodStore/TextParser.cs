using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MethodStore
{
    internal class TextParser
    {
        internal bool ParseClipboardTextToObjectMethods(ObjectMethod objectMethod)
        {
            string textClipboard = Clipboard.GetText();

            if (string.IsNullOrWhiteSpace(textClipboard))
                return false;

            textClipboard = RemoveStartText(textClipboard, "Процедура");
            textClipboard = RemoveStartText(textClipboard, "Функция");
            textClipboard = textClipboard.TrimStart();

            int countDot = textClipboard.Count(f => f == '.');
            if (countDot == 1)
            {
                int positionDot = textClipboard.IndexOf('.');

                objectMethod.Module = RemoveSpace(new string(textClipboard.Take(positionDot).ToArray()));

                textClipboard = RemoveStartText(textClipboard, objectMethod.Module + ".");

                ParseTextMethodNameAndParameters(objectMethod, textClipboard);

                return true;
            }
            else
            {
                int countOpeningBracket = textClipboard.Count(f => f == '(');
                if (countOpeningBracket == 1)
                {
                    ParseTextMethodNameAndParameters(objectMethod, textClipboard);

                    return true;
                }
                else
                    return false;
            }
        }

        private void ParseTextMethodNameAndParameters(ObjectMethod objectMethod, string textClipboard)
        {
            int countOpeningBracket = textClipboard.Count(f => f == '(');

            if (countOpeningBracket == 1)
            {
                int positionOpeningBracket = textClipboard.IndexOf("(");

                objectMethod.MethodName = RemoveSpace(textClipboard.Substring(0, positionOpeningBracket));

                textClipboard = textClipboard.Substring(positionOpeningBracket + 1);

                int countClosingBracket = textClipboard.Count(f => f == ')');

                if (countClosingBracket == 1)
                {
                    string textParameter = string.Empty;

                    textParameter = textClipboard;
                    textParameter = textParameter.Replace("\r", "");
                    textParameter = textParameter.Replace("\n", "");
                    textParameter = textParameter.Replace("\t", "");
                    textParameter = textParameter.TrimEnd(';');
                    textParameter = textParameter.TrimEnd(' ');
                    textParameter = RemoveEndText(textParameter, "Экспорт");
                    textParameter = textParameter.TrimEnd(' ');
                    textParameter = textParameter.TrimEnd(')');

                    List<Parameter> listParameters = new List<Parameter>();

                    string[] textParameters = textParameter.Split(',');
                    foreach (string item in textParameters)
                    {
                        if (string.IsNullOrWhiteSpace(item))
                            continue;


                        string textCurrentParameter = item.Replace("знач ", "Знач ");

                        textCurrentParameter = textCurrentParameter.TrimStart().TrimEnd();

                        Parameter newParameter = new Parameter();

                        if (textCurrentParameter.StartsWith("Знач "))
                        {
                            newParameter.ByValue = true;
                            textCurrentParameter = textCurrentParameter.Replace("Знач ", "");
                        }

                        int positionEquals = textCurrentParameter.IndexOf('=');
                        if (positionEquals > 0)
                        {
                            newParameter.Name = RemoveSpace(textCurrentParameter.Substring(0, positionEquals));
                            newParameter.ValueByDefault = TrimNotUserChar(textCurrentParameter.Substring(positionEquals));
                        }
                        else
                            newParameter.Name = RemoveSpace(textCurrentParameter);

                        listParameters.Add(newParameter);
                    }
                    objectMethod.Parameters = listParameters.ToArray();
                }
            }
            else
                objectMethod.MethodName = textClipboard;
        }

        private string RemoveStartText(string text, string find)
        {
            if (text.StartsWith(find))
                return text.Substring(find.Length);
            else
                return text;
        }

        private string RemoveEndText(string text, string find)
        {
            if (text.EndsWith(find))
                return text.Remove(text.Length - find.Length);
            else
                return text;
        }

        private string RemoveSpace(string text)
        {
            return text.Replace(" ", "");
        }

        private string TrimNotUserChar(string text)
        {
            string tempString = text.TrimStart().TrimStart('=').Trim();

            if (tempString != "\"\"")
                tempString = tempString.TrimStart('"').TrimEnd('"');

            return tempString;
        }

    }
}
