using Avalonia.Controls;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using NAudio.Utils;
using System;
using System.Runtime.InteropServices;

#pragma warning disable CS8625

namespace NuclearElephants.Utills;

public static class WallpaperUtils
{
    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern IntPtr FindWindow(string? lpClassName, string? lpWindowName);

    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter,
                                                string? lpszClass, string? lpszWindow);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SendMessageTimeout(IntPtr hWnd, uint Msg,
                                                      IntPtr wParam, IntPtr lParam,
                                                      uint fuFlags, uint uTimeout,
                                                      out IntPtr lpdwResult);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

    /// <summary>
    /// Locates (or spawns) the WorkerW window that sits behind the
    /// desktop icon layer. Returns IntPtr.Zero on failure.
    /// </summary>
    public static IntPtr Locate()
    {
        var progman = FindWindow("Progman", null);
        if (progman == IntPtr.Zero) return IntPtr.Zero;
        // Tell Progman to make the WorkerW. Spec says timeout 0x0000.
        SendMessageTimeout(progman, 0x052C, IntPtr.Zero, IntPtr.Zero, 0x0000, 1000, out _);

        // Walk siblings of Progman looking for the WorkerW that
        // contains a SHELLDLL_DefView child — the one BEHIND it is
        // our target.
        var workerw = FindWindowEx(progman, IntPtr.Zero, "WorkerW", null);
        var after = IntPtr.Zero;
        do
        {
            after = FindWindowEx(IntPtr.Zero, after, "WorkerW", null);
            var defview = FindWindowEx(after, IntPtr.Zero, "SHELLDLL_DefView", null);
            if (defview != IntPtr.Zero)
            {
                // The next WorkerW after this one is what we want.
                workerw = FindWindowEx(IntPtr.Zero, after, "WorkerW", null);
                break;
            }
        } while (after != IntPtr.Zero);

        return workerw;
    }

    /// <summary>
    /// Reparent the given window onto WorkerW so it renders
    /// below desktop icons.
    /// </summary>
    public static bool SetWallpaper(IntPtr ourHwnd)
    {
        var workerw = Locate();
        if (workerw == IntPtr.Zero) return false;
        SetParent(ourHwnd, workerw);
        return true;
    }
}
#pragma warning restore CS8625