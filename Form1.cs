using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace cdr
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        private Timer effectTimer;
        private Random random;

        public Form1()
        {
            InitializeComponent();

            this.DoubleBuffered = false;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            random = new Random();
            effectTimer = new Timer();
            effectTimer.Interval = 10;
            effectTimer.Tick += EffectTimer_Tick;
            effectTimer.Start();
        }

        private void EffectTimer_Tick(object sender, EventArgs e)
        {
            IntPtr desktopDC = GetDC(IntPtr.Zero);
            using (Graphics g = Graphics.FromHdc(desktopDC))
            {
                int x = random.Next(-50, Screen.PrimaryScreen.Bounds.Width);
                int y = random.Next(-50, Screen.PrimaryScreen.Bounds.Height);
                int width = random.Next(100, 400);
                int height = random.Next(100, 300);

                g.CopyFromScreen(x, y, x + random.Next(-10, 11), y + random.Next(-10, 11), new Size(width, height));
            }
            ReleaseDC(IntPtr.Zero, desktopDC);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
        }
    }
}
