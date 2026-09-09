using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using static Model.CanvasManage;

namespace Model;

public static class Mouse
{
    const int WH_MOUSE_LL = 14;

    delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern IntPtr GetModuleHandle(string? lpModuleName);

    // Функция для установки позиции курсора
    [DllImport("user32.dll")]
    static extern bool SetCursorPos(int x, int y);

    // Функция для отправки событий мыши
    [DllImport("user32.dll")]
    static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, IntPtr dwExtraCommand);

    const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
    const uint MOUSEEVENTF_LEFTUP = 0x0004;
    const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
    const uint MOUSEEVENTF_RIGHTUP = 0x0010;

    static LowLevelMouseProc _proc = HookCallback;
    static IntPtr _hookID = IntPtr.Zero;

    public static void StartHook()
    {
        _hookID = SetHook(_proc);
    }

    public static void StopHook()
    {
        UnhookWindowsHookEx(_hookID);
    }

    private static IntPtr SetHook(LowLevelMouseProc proc)
    {
        using (Process curProcess = Process.GetCurrentProcess())
        using (ProcessModule? curModule = curProcess.MainModule)
        {
            return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(curModule?.ModuleName), 0);
        }
    }

#pragma warning disable CS8605 // Unboxing a possibly null value.
    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0)
        {
            MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));


            switch ((Global.KeyMessages)wParam)
            {
                case Global.KeyMessages.WM_LBUTTONDOWN:
                Write.Line($"Left button down at X={hookStruct.pt.x}, Y={hookStruct.pt.y}");
                break;
                case Global.KeyMessages.WM_RBUTTONDOWN:
                Write.Line($"Right button down at X={hookStruct.pt.x}, Y={hookStruct.pt.y}");
                break;
            }

            Write.Line($"Mouse at X={hookStruct.pt.x}, Y={hookStruct.pt.y}");
        }
        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }
#pragma warning restore CS8605 // Unboxing a possibly null value.

    // Методы для управления мышью
    public static void MoveMouseTo(int x, int y)
    {
        SetCursorPos(x, heightScreen - y);
    }

    public static void LeftClickAt(int x, int y)
    {
        SetCursorPos(x, heightScreen - y);
        mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)x, (uint)(heightScreen - y), 0, IntPtr.Zero);
    }

    public static void RightClickAt(int x, int y)
    {
        SetCursorPos(x, heightScreen - y);
        mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, (uint)x, (uint)(heightScreen - y), 0, IntPtr.Zero);
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct POINT
    {
        public int x;
        public int y;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MSLLHOOKSTRUCT
    {
        public POINT pt;
        public uint mouseData;
        public uint flags;
        public uint time;
        public UIntPtr dwExtraCommand;
    }
}