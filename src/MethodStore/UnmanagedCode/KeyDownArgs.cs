using System;
using System.Windows.Input;

namespace MethodStore.UnmanagedCode
{
    internal class KeyDownArgs : EventArgs
    {
        public Key KeyDown { get; private set; }

        internal KeyDownArgs(Key key)
        {
            KeyDown = key;
        }
    }
}
