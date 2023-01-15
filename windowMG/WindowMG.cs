using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace windowMG
{
    public class WindowMG
    {
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_SHOWWINDOW = 0x0040;
        private const int SWP_NOZORDER = 0x0004;
        private const int HWND_TOPMOST = -1;
        private const int HWND_NOTOPMOST = -2;

        private const int SRCCOPY = 13369376;
        private const int DWMWA_EXTENDED_FRAME_BOUNDS = 9;
        //Win32API Import
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int EnumWindows(EnumWindowsDelegate lpEnumFunc, IntPtr lparam);
        public delegate bool EnumWindowsDelegate(IntPtr hWnd, IntPtr lparam);
        [DllImport("user32")]
        private static extern bool IsWindowVisible(IntPtr hWnd);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowTextLength(IntPtr hWnd);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GetClientRect(IntPtr hwnd, out RECT lpRect);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hwnd, int hwndInsertAfter, int x, int y, int cx, int cy, uint flags);

        private List<KeyValuePair<String, IntPtr>> windowList = new List<KeyValuePair<String, IntPtr>>();

        public KeyValuePair<String, IntPtr>[] windowListUpdate()
        {
            windowList.Clear();
            bool EnumWindowCallBack(IntPtr hWnd, IntPtr lparam)
            {
                //https://www.natsuneko.blog/entry/2018/08/09/002742
                //不可視化ウィンドウ除外
                if (!IsWindowVisible(hWnd))
                    return true;

                // タイトル空
                var title = new StringBuilder(1024);
                GetWindowText(hWnd, title, title.Capacity);
                if (string.IsNullOrWhiteSpace(title.ToString()))
                    return true; // Skipped

                int textLen = GetWindowTextLength(hWnd);
                if (0 < textLen)
                {
                    StringBuilder tsb = new StringBuilder(textLen + 1);
                    GetWindowText(hWnd, tsb, tsb.Capacity);
                    windowList.Add(new KeyValuePair<String, IntPtr>(tsb.ToString(), hWnd));
                }

                //すべてのウィンドウを列挙する
                return true;
            }
            EnumWindows(EnumWindowCallBack, IntPtr.Zero);
            return windowList.ToArray();
        }
        public bool setWindowSize(int targetWindowIndex, int x, int y, bool windowFrameInclude)
        {
            RECT W, C;
            IntPtr target;
            try
            {
                target = windowList[targetWindowIndex].Value;
            }
            catch (Exception e)
            {
                return false;
            }
            GetWindowRect(target, out W);
            GetClientRect(target, out C);
            if (!windowFrameInclude)
            {
                x += (W.right - W.left) - (C.right - C.left);
                y += (W.bottom - W.top) - (C.bottom - C.top);
            }
            SetWindowPos(target, -2, 0, 0, x, y, SWP_NOMOVE | SWP_NOZORDER);
            return true;
        }

        public Size getWindowSize(int targetWindowIndex, bool windowFrameInclude)
        {
            RECT C;
            IntPtr target;
            try
            {
                target = windowList[targetWindowIndex].Value;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }

            if (windowFrameInclude)
                GetWindowRect(target, out C);
            else
                GetClientRect(target, out C);

            return new Size(C.right - C.left, C.bottom - C.top);
        }
    }


}