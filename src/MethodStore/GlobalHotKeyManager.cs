using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MethodStore
{
    internal class GlobalHotKeyManager : IDisposable
    {
        private GlobalHotKeyEvents _globalHotKeyEvents;
        private LowLevelKeyboardListener _listener;

        internal bool PressedLeftCtrl { get; private set; }
        internal bool PressedLeftAlt { get; private set; }
        internal bool PressedF { get; private set; }
        internal bool PressedC { get; private set; }

        private const Key _keyLeftCtrl = Key.LeftCtrl;
        private const Key _keyLeftAlt = Key.LeftAlt;
        private const Key _keyF = Key.F;
        private const Key _keyC = Key.C;

        internal GlobalHotKeyManager(GlobalHotKeyEvents globalHotKeyEvents)
        {
            _globalHotKeyEvents = globalHotKeyEvents;

            _listener = new LowLevelKeyboardListener();
            _listener.OnKeyDown += _listener_OnKeyDown;
            _listener.OnKeyUp += _listener_OnKeyUp;

            _listener.HookKeyboard();
        }

        void _listener_OnKeyDown(object sender, KeyDownArgs e)
        {
            switch (e.KeyDown)
            {
                case _keyLeftCtrl:
                    PressedLeftCtrl = true;
                    break;
                case _keyLeftAlt:
                    PressedLeftAlt = true;
                    break;
                case _keyF:
                    PressedF = true;
                    break;
                case _keyC:
                    PressedC = true;
                    break;
            }

            if (PressedLeftCtrl && PressedLeftAlt)
            {
                if (PressedF)
                {
                    PressedLeftCtrl = false;
                    PressedLeftAlt = false;
                    PressedF = false;

                    _globalHotKeyEvents.EvokeOpenFormMainMenuMethodEvent();
                }
                else if (PressedC)
                {
                    PressedLeftCtrl = false;
                    PressedLeftAlt = false;
                    PressedC = false;

                    _globalHotKeyEvents.EvokeOpenFormObjectMethodEvent();
                }

            }
        }

        void _listener_OnKeyUp(object sender, KeyUpArgs e)
        {
            switch (e.KeyUp)
            {
                case _keyLeftCtrl:
                    PressedLeftCtrl = false;
                    break;
                case _keyLeftAlt:
                    PressedLeftAlt = false;
                    break;
                case _keyF:
                    PressedF = false;
                    break;
                case _keyC:
                    PressedC = false;
                    break;
            }
        }

        public void Dispose()
        {
            _listener.UnHookKeyboard();
        }

        internal class LowLevelKeyboardListener
        {
            private const int WH_KEYBOARD_LL = 13;
            private const int WM_KEYDOWN = 0x0100;
            private const int WM_KEYUP = 0x0101;
            private const int WM_SYSKEYDOWN = 0x0104;

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool UnhookWindowsHookEx(IntPtr hhk);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern IntPtr GetModuleHandle(string lpModuleName);

            public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

            public event EventHandler<KeyDownArgs> OnKeyDown;
            public event EventHandler<KeyUpArgs> OnKeyUp;

            private LowLevelKeyboardProc _proc;
            private IntPtr _hookID = IntPtr.Zero;

            public LowLevelKeyboardListener()
            {
                _proc = HookCallback;
            }

            public void HookKeyboard()
            {
                _hookID = SetHook(_proc);
            }

            public void UnHookKeyboard()
            {
                UnhookWindowsHookEx(_hookID);
            }

            private IntPtr SetHook(LowLevelKeyboardProc proc)
            {
                using (Process curProcess = Process.GetCurrentProcess())
                using (ProcessModule curModule = curProcess.MainModule)
                {
                    return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
                }
            }

            private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
            {
                if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN)
                {
                    int vkCode = Marshal.ReadInt32(lParam);

                    OnKeyDown?.Invoke(this, new KeyDownArgs(KeyInterop.KeyFromVirtualKey(vkCode)));
                }
                else if (nCode >= 0 && wParam == (IntPtr)WM_KEYUP || wParam == (IntPtr)WM_SYSKEYDOWN)
                {
                    int vkCode = Marshal.ReadInt32(lParam);

                    OnKeyUp?.Invoke(this, new KeyUpArgs(KeyInterop.KeyFromVirtualKey(vkCode)));
                }

                return CallNextHookEx(_hookID, nCode, wParam, lParam);
            }
        }

        internal class KeyDownArgs : EventArgs
        {
            public Key KeyDown { get; private set; }

            internal KeyDownArgs(Key key)
            {
                KeyDown = key;
            }
        }

        internal class KeyUpArgs : EventArgs
        {
            public Key KeyUp { get; private set; }

            internal KeyUpArgs(Key key)
            {
                KeyUp = key;
            }
        }
    }
}
