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

            textClipboard = textClipboard.TrimStart("Процедура ".ToCharArray());
            textClipboard = textClipboard.TrimStart("Функция ".ToCharArray());

            int countDot = textClipboard.Count(f => f == '.');
            if (countDot == 1)
            {
                int positionDot = textClipboard.IndexOf('.');

                objectMethod.Module = new string(textClipboard.Take(positionDot).ToArray());

                textClipboard = textClipboard.TrimStart(
                    (objectMethod.Module + ".").ToCharArray());

                objectMethod.Module = RemoveSpace(objectMethod.Module);

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

                objectMethod.MethodName = new string(textClipboard.Take(positionOpeningBracket).ToArray()).Replace(" ", "");

                textClipboard = textClipboard.TrimStart(
                    (objectMethod.MethodName + "(").ToCharArray());

                objectMethod.MethodName = RemoveSpace(objectMethod.MethodName);

                int countClosingBracket = textClipboard.Count(f => f == ')');

                if (countClosingBracket == 1)
                {
                    string textParameter = string.Empty;
                    textParameter = textClipboard.TrimEnd(' ');
                    textParameter = textParameter.TrimEnd("Экспорт".ToCharArray());
                    textParameter = textParameter.TrimEnd(' ');
                    textParameter = textParameter.TrimEnd(")".ToCharArray());

                    List<Parameter> listParameters = new List<Parameter>();

                    string[] textParameters = textParameter.Split(',');
                    foreach (string item in textParameters)
                    {
                        if (string.IsNullOrWhiteSpace(item))
                            continue;

                        string textCurrentParameter = item;

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
                            newParameter.Name = textCurrentParameter.Substring(0, positionEquals);
                            newParameter.ValueByDefault = TrimNotUserChar(
                                textCurrentParameter.TrimStart(
                                    newParameter.Name.ToCharArray()));
                            newParameter.Name = RemoveSpace(newParameter.Name);
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

        private string RemoveSpace(string text)
        {
            return text.Replace(" ", "");
        }

        private string TrimNotUserChar(string text)
        {
            return text.TrimStart().TrimStart('=').TrimStart().TrimEnd().TrimStart('"').TrimEnd('"');
        }

    }
}
