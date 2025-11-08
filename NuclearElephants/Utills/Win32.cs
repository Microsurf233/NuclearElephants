using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

// Reference: https://github.com/Yinmany/WinWallpaper
namespace NuclearElephants.Utills
{
    // win32api
    public class Win32
    {
        public class User32
        {
            /// <summary>
            ///  查找顶级窗口句柄
            /// </summary>
            /// <param name="className">类名</param>
            /// <param name="titleName">标题</param>
            /// <returns></returns>
            [DllImport("user32.dll")]
            public static extern IntPtr FindWindow(string className, string titleName);

            /// <summary>
            /// 查找子窗口句柄
            /// </summary>
            /// <param name="hwndParent">要查找窗口的父句柄</param>
            /// <param name="hwndChildAfter">从这个窗口后开始查找</param>
            /// <param name="className">窗口类名</param>
            /// <param name="title">窗口标题</param>
            /// <returns>找到返回窗口句柄，没找到返回0</returns>
            [DllImport("user32.dll")]
            public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string className, string title);

            /// <summary>
            /// 枚举窗口
            /// </summary>
            /// <param name="lpEnumFunc"></param>
            /// <param name="lParam"></param>
            /// <returns></returns>
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool EnumWindows(EnumWindowProc lpEnumFunc, IntPtr lParam);
            
            public delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);
            
            [DllImport("user32")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr i);

            /// <summary>
            /// 改变指定子窗口的父窗口
            /// </summary>
            /// <param name="hwndChild">子窗口句柄</param>
            /// <param name="newParent">新的父窗口句柄 如果该参数是NULL，则桌面窗口就成为新的父窗口</param>
            /// <returns>如果函数成功，返回值为子窗口的原父窗口句柄；如果函数失败，返回值为NULL</returns>
            [DllImport("user32.dll")]
            public static extern IntPtr SetParent(IntPtr hwndChild, IntPtr newParent);

            /// <summary>
            /// 显示窗口异步
            /// </summary>
            /// <param name="hWnd">窗口句柄</param>
            /// <param name="cmdShow">显示方式</param>
            [DllImport("user32.dll")]  
            public static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
            [DllImport("user32.dll")]
            public static extern bool ShowWindow(IntPtr hWnd, int cmdShow);
            public const int SW_SHOW = 5;
            public const int SW_HIDE = 0;
            public const int WS_SHOWNORMAL = 1;
            
            /// <summary>
            /// 该函数返回指定窗口的边框矩形的尺寸。该尺寸以相对于屏幕坐标左上角的屏幕坐标给出
            /// </summary>
            /// <param name="hwnd">窗口句柄</param>
            /// <param name="rect">指向一个RECT结构的指针，该结构接收窗口的左上角和右下角的屏幕坐标</param>
            [DllImport("user32.dll")]
            public static extern void GetWindowRect(IntPtr hwnd,ref Rectangle rect);

            [DllImport("user32.dll")]
            public static extern int SetWindowPos(IntPtr hWnd,IntPtr hWndlnsertAfter,int x, int y ,int cx,int cy,uint flag);
            public const int HWND_TOP = 0; // 在前面
            public const int HWND_BOTTOM = 1; // 在后面
            public const int  HWND_TOPMOST = -1; // 在前面, 位于任何顶部窗口的前面
            public const int HWND_NOTOPMOST = -2; // 在前面, 位于其他顶部窗口的后面}
            
            public const uint SWP_NOSIZE     = 0x0001;
            public const uint SWP_NOMOVE     = 0x0002;
            public const uint SWP_NOZORDER   = 0x0004;
            public const uint SWP_NOACTIVATE = 0x0010;
            public const uint SWP_SHOWWINDOW = 0x0040;


            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr SendMessageTimeout(IntPtr windowHandle, uint Msg, IntPtr wParam, IntPtr lParam, SendMessageTimeoutFlags flags, uint timeout, out IntPtr result);

            [Flags]
            public enum SendMessageTimeoutFlags : uint
            {
                SMTO_ABORTIFHUNG = 2,
                SMTO_BLOCK = 1,
                SMTO_ERRORONEXIT = 0x20,
                SMTO_NORMAL = 0,
                SMTO_NOTIMEOUTIFNOTHUNG = 8
            }
            

            [DllImport("user32.dll", SetLastError = true)]
            public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

            [DllImport("user32.dll", SetLastError = true)]
            public static extern long SetWindowLongA(IntPtr hWnd, int nIndex, int dwNewLong);
            
            public const int WS_EX_LAYERED = 0x00080000;
            
            public const int GWL_EXSTYLE = -20;
            


            /// <summary>
            /// 设置分层窗口的透明度或颜色键。
            /// </summary>
            /// <param name="hWnd">窗口句柄</param>
            /// <param name="crKey">颜色键（LWA_COLORKEY 时生效）</param>
            /// <param name="bAlpha">透明度 0-255（LWA_ALPHA 时生效）</param>
            /// <param name="dwFlags">LWA_* 标志</param>
            [DllImport("user32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool SetLayeredWindowAttributes(
                IntPtr hWnd,            
                uint   crKey,           
                byte   bAlpha,          
                uint   dwFlags);        
            public const uint LWA_COLORKEY = 0x00000001;
            public const uint LWA_ALPHA    = 0x00000002;
            
            [DllImport("user32.dll")]
            public static extern IntPtr GetDesktopWindow();
        }
    }



}