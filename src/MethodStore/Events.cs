using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodStore
{
    internal delegate void RefreshDataGridEvent();

    internal class RefreshDataGridEvents : EventArgs
    {
        internal event RefreshDataGridEvent RefreshDataGrid;

        internal void EvokeRefreshDataGrid()
        {
            if (RefreshDataGrid == null)
                return;

            RefreshDataGrid();
        }
    }


    internal delegate void UpdateVersion(bool NeedNotified);

    internal class CallUpdateListObjectMethodsEvents: EventArgs
    {
        internal event UpdateVersion CallUpdateListObjectMethods;

        internal bool NeedNotified { get; set; }

        internal void EvokeUpdateList()
        {
            if (CallUpdateListObjectMethods == null)
                return;

            CallUpdateListObjectMethods(NeedNotified);
        }
    }


    internal delegate void GlobalHotKeyEvent();

    internal class GlobalHotKeyEvents : EventArgs
    {
        internal event GlobalHotKeyEvent GlobalHotKeyEvent;

        internal void EvokeGlobalHotKeyEvent()
        {
            if (GlobalHotKeyEvent == null)
                return;

            GlobalHotKeyEvent();
        }
    }

}
