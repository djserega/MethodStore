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
        internal bool ParseClipboardToModuleAndMethodName(ObjectMethod objectMethod)
        {
            string textClipboard = Clipboard.GetText();

            if (string.IsNullOrWhiteSpace(textClipboard))
                return false;

            int countDot = textClipboard.Count(f => f == '.');
            if (countDot == 1)
            {
                int positionDot = textClipboard.IndexOf('.');

                objectMethod.Module = new string(textClipboard.Take(positionDot).ToArray());

                textClipboard = textClipboard.TrimStart(
                    (objectMethod.Module + ".").ToCharArray());

                int countOpeningBracket = textClipboard.Count(f => f == '(');

                if (countOpeningBracket == 1)
                {
                    int positionOpeningBracket = textClipboard.IndexOf("(");

                    objectMethod.MethodName = new string(textClipboard.Take(positionOpeningBracket).ToArray());

                    textClipboard = textClipboard.TrimStart(
                        (objectMethod.MethodName + "(").ToCharArray());

                    int countClosingBracket = textClipboard.Count(f => f == ')');

                    if (countClosingBracket == 1)
                    {
                        string textParameter = textClipboard.TrimEnd(");".ToCharArray());

                        List<Parameter> listParameters = new List<Parameter>();

                        string[] textParameters = textParameter.Replace(" ", "").Split(',');
                        foreach (string item in textParameters)
                        {
                            listParameters.Add(new Parameter(item));
                        }
                        objectMethod.Parameters = listParameters.ToArray();
                    }
                }
                else
                    objectMethod.MethodName = textClipboard;

                return true;
            }
            else
                return false;
        }
    }
}
