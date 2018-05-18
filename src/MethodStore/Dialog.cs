using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodStore
{
    internal static class Dialog
    {
        internal static void ShowMessage(string textMessage, int timer = 10)
            => new WindowMessage(textMessage, timer).Show();

        internal static bool? ShowQuesttion(string textMessage, int timer = 10)
        {
            bool? resutQuestion = null;

            WindowMessage form = new WindowMessage(textMessage, timer, true);
            form.ShowDialog();
            if (form.PressButtonOK)
                resutQuestion = true;
            else if (form.PressButtonCancel)
                resutQuestion = false;

            return resutQuestion;
        }
    }
}
