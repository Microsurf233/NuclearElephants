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

        // 获取 Proman
        var proman = FindWindow("Progman", null);

        // 发送消息 生成 WorkerW
        SendMessageTimeout(proman, 0x52c, new IntPtr(0), IntPtr.Zero, SendMessageTimeoutFlags.SMTO_NORMAL, 0x3e8, out var zero);

        // 0x52c消息会生成两个WorkerW 所以要枚举不包含“SHELLDLL_DefView”这个的 WorkerW 窗口 隐藏掉。
        var workerwWithoutDefView = IntPtr.Zero;
        EnumWindows(EnumWorkerWWithoutDefView, IntPtr.Zero);
        ShowWindow(workerwWithoutDefView, SW_HIDE);

        // 将选定的壁纸窗口的 Parent 设定为获取到的 Proman
        SetParent(appWindowHandle, proman);
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
        // 获取 Proman
        var proman = FindWindow("Progman", null);

        // 发送消息 生成 WorkerW
        SendMessageTimeout(proman, 0x52c, new IntPtr(0), IntPtr.Zero, SendMessageTimeoutFlags.SMTO_NORMAL, 0x3e8, out var zero);
        
        var defView = IntPtr.Zero;
        EnumChildWindows(proman, (hwnd, lparam) =>
        {
            var h = FindWindowEx(hwnd, IntPtr.Zero, "SHELLDLL_DefView", null);
            if (h == IntPtr.Zero) return true; //继续枚举
            defView = h;
            return false; //停止枚举
        }, IntPtr.Zero);
        
        var workerW = IntPtr.Zero;
        EnumChildWindows(proman, (hwnd, lparam) =>
        {
            var h = FindWindowEx(hwnd, IntPtr.Zero, "WorkerW", null);
            if (h == IntPtr.Zero) return true; //继续枚举
            workerW = h;
            return false; //停止枚举
        }, IntPtr.Zero);

        var msgb = MessageBoxManager.GetMessageBoxStandard("title", $"def:{defView != IntPtr.Zero}\nw:{workerW != IntPtr.Zero}");
        msgb.ShowWindowAsync();
        
        // 设置扩展窗口样式(Layered)
        SetWindowLongA(appWindowHandle, GWL_EXSTYLE, WS_EX_LAYERED);
        
        // 设置透明窗口
        SetLayeredWindowAttributes(appWindowHandle, 0, 255, LWA_ALPHA);
        
        // 设置为 Proman 的子窗口
        SetParent(appWindowHandle, proman);
        
        // 1) 把壁纸窗口插到图标层下方
        SetWindowPos(appWindowHandle, defView, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
        
        // 2) 把系统 WorkerW 压到最底
        SetWindowPos(workerW, appWindowHandle, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
        
        ShowWindow(appWindowHandle, SW_SHOW);
    }
}
#pragma warning restore CS8625