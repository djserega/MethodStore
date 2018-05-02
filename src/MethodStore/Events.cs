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
}
