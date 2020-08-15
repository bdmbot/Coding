using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;

namespace BDMBOT
{
    class AI
    {
        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_LBUTTONUP = 0x202;      
        private const int WM_MOUSEMOVE = 0x0200;

        private static UInt32 MakeLParam(int LoWord, int HiWord)
        {
            return (UInt32)((HiWord << 16) | (LoWord & 0xFFFF));
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool PostMessage(IntPtr hWnd, int Msg, int wParam, uint lParam);

        // เลื่อนหน้าจอ
        public static void SwapDrag(IntPtr hWnd, IntPtr hWnd2, int x, int y, int step = 5, int run = 100, string move = "right")
        {
            int iRun = (run / step);
            int iStep = step;
            int iSumX = 0;
            int iSumY = 0;

            PostMessage(hWnd, WM_LBUTTONDOWN, 1, MakeLParam(x, y));

            Thread.Sleep(100);
            for (int i = 0; i < iRun; i++)
            {
                if (move == "left")
                {
                    iSumX = x - (i * iStep);
                    iSumY = y;
                }
                else
                if (move == "right")
                {
                    iSumX = x + (i * iStep);
                    iSumY = y;
                }
                else
                if (move == "up")
                {
                    iSumX = x;
                    iSumY = y - (i * iStep);
                }
                else
                if (move == "down")
                {
                    iSumX = x;
                    iSumY = y + (i * iStep);
                }
                PostMessage(hWnd, WM_MOUSEMOVE, 1, MakeLParam(iSumX, iSumY));
                FillEllipse(hWnd2, Color.LimeGreen, iSumX, iSumY, 5);
                Thread.Sleep(10);
            }
            Thread.Sleep(100);
            PostMessage(hWnd, (WM_LBUTTONUP), 0, MakeLParam(iSumX, iSumY));
            FillEllipse(hWnd2, Color.LimeGreen, iSumX, iSumY, 5);
            ClearGraphics(hWnd2);
        }
        public static void FillEllipse(IntPtr iHandle, Color color, int X, int Y, int rang = 5)
        {
            Rectangle rect = Rectangle.FromLTRB(X - rang, Y - rang, X + rang, Y + rang);
            Graphics g = Graphics.FromHwnd(iHandle);
            SolidBrush myBrush = new SolidBrush(color);
            g.FillEllipse(myBrush, rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
            g.Dispose();
        }
        public static void ClearGraphics(IntPtr Hwnd)
        {
            IntPtr hwnds = Hwnd;
            Graphics g = Graphics.FromHwnd(hwnds);
            g.Clear(Color.Wheat);
            g.Dispose();
        }
    }
}