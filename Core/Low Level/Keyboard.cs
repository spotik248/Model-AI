using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Model;

static class Keyboard
{
    const int WH_KEYBOARD_LL = 13;

    delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern IntPtr GetModuleHandle(string lpModuleName);

    // Имитация нажатия клавиш
    [DllImport("user32.dll")]
    static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraCommand);

    const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
    const uint KEYEVENTF_KEYUP = 0x0002;

    static LowLevelKeyboardProc _proc = HookCallback;
    static IntPtr _hookID = IntPtr.Zero;

    public static void StartHook()
    {
        _hookID = SetHook(_proc);
    }

    public static void StopHook()
    {
        UnhookWindowsHookEx(_hookID);
    }

#pragma warning disable CS8602 // Dereference of a possibly null reference.

    private static IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using (Process curProcess = Process.GetCurrentProcess())
        using (ProcessModule? curModule = curProcess.MainModule)
        {

            return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);

        }
    }

#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8605 // Unboxing a possibly null value.

    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0)
        {

            KBDLLHOOKSTRUCT hookStruct = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));


            switch ((Global.KeyMessages)wParam)
            {
                case Global.KeyMessages.WM_KEYDOWN:
                Write.Line($"Key Down: VK={hookStruct.vkCode}");
                break;
                case Global.KeyMessages.WM_KEYUP:
                Write.Line($"Key Up: VK={hookStruct.vkCode}");
                break;
            }
        }
        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }

#pragma warning restore CS8605 // Unboxing a possibly null value.

    /// <summary>
    /// Нажатие виртуальной клавиши (VK-код)
    /// </summary>
    public static void PressKey(byte vkCode)
    {
        keybd_event(vkCode, 0, KEYEVENTF_EXTENDEDKEY, UIntPtr.Zero);
    }

    /// <summary>
    /// Отпустить виртуальную клавишу
    /// </summary>
    public static void ReleaseKey(byte vkCode)
    {
        keybd_event(vkCode, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, UIntPtr.Zero);
    }

    /// <summary>
    /// Нажать и отпустить виртуальную клавишу
    /// </summary>
    public static void JumpKey(byte vkCode)
    {
        keybd_event(vkCode, 0, KEYEVENTF_EXTENDEDKEY, UIntPtr.Zero);
        
        keybd_event(vkCode, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, UIntPtr.Zero);
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct KBDLLHOOKSTRUCT
    {
        public int vkCode;
        public int scanCode;
        public int flags;
        public int time;
        public UIntPtr dwExtraCommand;
    }
}