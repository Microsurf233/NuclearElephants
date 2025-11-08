using System;
using Avalonia.Controls;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using static NuclearElephants.Utills.Win32.User32;

#pragma warning disable CS8625

namespace NuclearElephants.Utills;

public static class WallpaperUtils
{
    /// <summary>
    /// 获取窗口句柄
    /// </summary>
    public static void SetWallpaper(Window window)
    {
        if (Environment.OSVersion.Version.Build >= 26002)
        {
            SetWallpaperNew(window);
        }
        else
        {
            SetWallpaperOld(window);
        }
    }

    private static void SetWallpaperOld(Window window)
    {
        var appWindowHandle = window.TryGetPlatformHandle()?.Handle ?? IntPtr.Zero;
        if (appWindowHandle == IntPtr.Zero)
        {
            throw new Exception("Failed to get platform handle.");
        }

        // 获取 Progman
        var progman = FindWindow("Progman", null);

        // 发送消息 生成 WorkerW
        SendMessageTimeout(progman, 0x52c, new IntPtr(0), IntPtr.Zero, SendMessageTimeoutFlags.SMTO_NORMAL, 0x3e8, out var zero);

        // 0x52c消息会生成两个WorkerW 所以要枚举不包含“SHELLDLL_DefView”这个的 WorkerW 窗口 隐藏掉。
        var workerwWithoutDefView = IntPtr.Zero;
        EnumWindows(EnumWorkerWWithoutDefView, IntPtr.Zero);
        ShowWindow(workerwWithoutDefView, SW_HIDE);

        // 将选定的壁纸窗口的 Parent 设定为获取到的 Proman
        SetParent(appWindowHandle, progman);
        return;

        bool EnumWorkerWWithoutDefView(IntPtr hwnd, IntPtr lParam)
        {
            if (FindWindowEx(hwnd, IntPtr.Zero, "SHELLDLL_DefView", null) != IntPtr.Zero)
                workerwWithoutDefView = FindWindowEx(IntPtr.Zero, hwnd, "WorkerW", null);
            return true;
        }
    }
    
    private static void SetWallpaperNew(Window window)
    {
        var appWindowHandle = window.TryGetPlatformHandle()?.Handle ?? IntPtr.Zero;
        if (appWindowHandle == IntPtr.Zero)
            throw new Exception("Failed to get platform handle.");

        // 获取 Progman
        var progman = FindWindow("Progman", null);

        // 发送消息，要求系统生成 WorkerW
        SendMessageTimeout(progman, 0x052C, IntPtr.Zero, IntPtr.Zero, SendMessageTimeoutFlags.SMTO_NORMAL, 0x3e8, out _);

        var workerW = FindWindowEx(progman, IntPtr.Zero, "WorkerW", null);
        var defView = FindWindowEx(progman, IntPtr.Zero, "SHELLDLL_DefView", null);
        if (defView == IntPtr.Zero | workerW == IntPtr.Zero)
        {
            do
            {
                workerW = FindWindowEx(progman, defView, "WorkerW", null);
                defView = FindWindowEx(workerW, IntPtr.Zero, "SHELLDLL_DefView", null);
            } while (defView == IntPtr.Zero && workerW != IntPtr.Zero);
        }

        // 把壁纸窗口嵌入正确的 WorkerW，不遮挡桌面图标
        SetParent(appWindowHandle, workerW);

        // 设置扩展样式 Layered
        SetWindowLongA(appWindowHandle, GWL_EXSTYLE, WS_EX_LAYERED);

        // 设置为透明
        SetLayeredWindowAttributes(appWindowHandle, 0, 0, LWA_COLORKEY);
        
        _ = SetWindowPos(appWindowHandle, defView, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_NOACTIVATE);

    }
}
#pragma warning restore CS8625