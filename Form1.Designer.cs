namespace cdr
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Timer designTimer;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.designTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();

            this.designTimer.Interval = 15;
            this.designTimer.Tick += new System.EventHandler(this.DesignTimer_Tick);

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.Text = "cdr";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.ResumeLayout(false);
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            this.designTimer.Start();
        }

        private void DesignTimer_Tick(object sender, System.EventArgs e)
        {
            this.Invalidate();
        }

        private void Form1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            System.Random rand = new System.Random();
            e.Graphics.Clear(System.Drawing.Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256)));

            using (System.Drawing.Font font = new System.Drawing.Font("Courier New", rand.Next(10, 70), System.Drawing.FontStyle.Bold))
            {
                for (int i = 0; i < 30; i++)
                {
                    char asciiChar = (char)rand.Next(33, 127);
                    using (System.Drawing.Brush brush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256))))
                    {
                        float x = (float)rand.NextDouble() * this.Width;
                        float y = (float)rand.NextDouble() * this.Height;
                        e.Graphics.DrawString(asciiChar.ToString(), font, brush, x, y);
                    }
                }
            }
        }
    }
}
