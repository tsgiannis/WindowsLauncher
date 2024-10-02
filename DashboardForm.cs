// File: DashboardForm.cs
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace DashboardApp
{
    public partial class DashboardForm : Form
    {
        // P/Invoke declaration for SetWindowPos
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_SHOWWINDOW = 0x0040;
        private static readonly IntPtr HWND_TOP = new IntPtr(0);
        public DashboardForm()
        {
            InitializeComponent();
            this.Icon = WindowsLauncher.Properties.Resources.JohnTsioumpris;
            this.WindowState = FormWindowState.Normal;
        }
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            // Force window state to normal
            this.WindowState = FormWindowState.Normal;

            // Restore and bring the window to the front
            SetWindowPos(this.Handle, HWND_TOP, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_SHOWWINDOW);
        }

        private void InitializeComponent()
        {
            this.Text = "Dashboard";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Create buttons
            CreateButton("Quote Offer", Color.FromArgb(255, 107, 107), new Rectangle(100, 150, 280, 150), WindowsLauncher.Properties.Resources.Quote,"btnQuote");
            CreateButton("Stock Management", Color.FromArgb(78, 205, 196), new Rectangle(420, 150, 280, 150), WindowsLauncher.Properties.Resources.StockManagement,"btnStock");
            CreateButton("Sales", Color.FromArgb(255, 165, 0), new Rectangle(100, 350, 600, 150), WindowsLauncher.Properties.Resources.Sales,"btnSales");

            // Create logo
            CreateLogoAndLabel();

            // Set up the form's paint event to draw the gradient background
            this.Paint += new PaintEventHandler(DashboardForm_Paint);
        }
        private Image ResizeImage(Image imgToResize, int width, int height)
        {
            Bitmap b = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(b))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(imgToResize, 0, 0, width, height);
            }
            return b;
        }
        private void CreateButton(string text, Color color, Rectangle bounds, Bitmap bitmap, String btnName)
        {
            Button button = new Button();
            button.Text = text;
            button.Font = new Font("Arial", 14, FontStyle.Bold);
            button.ForeColor = Color.White;
            button.BackColor = color;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Bounds = bounds;
            button.Click += Button_Click;
            button.TextAlign = ContentAlignment.BottomCenter;
            button.TextImageRelation = TextImageRelation.ImageAboveText;
            int imageSize = (int)(bounds.Height * 0.6);
            button.Image = ResizeImage(bitmap, imageSize, imageSize);
            button.ImageAlign = ContentAlignment.TopCenter;
            button.Name = btnName;
            this.Controls.Add(button);
        }
        private void companyLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Extract the URL from the link data
            string target = e.Link.LinkData as string;

            if (!string.IsNullOrEmpty(target))
            {
                // Open the URL in the default browser
                Process.Start(new ProcessStartInfo(target) { UseShellExecute = true });
            }
        }
        private void CreateLogoAndLabel()
        {
            PictureBox logo = new PictureBox();
            logo.Size = new Size(80, 80);
            logo.Location = new Point(20, 20);
            logo.SizeMode = PictureBoxSizeMode.Zoom;
            logo.Image = new Bitmap(WindowsLauncher.Properties.Resources.meOpenToWork);

            this.Controls.Add(logo);
            // Create label under logo
            LinkLabel companyLabel = new LinkLabel();
            companyLabel.Text = "My LinkedIn profile";
            // Add an event handler to do something when the links are clicked.
            companyLabel.Links.Add(0, 20, "https://www.linkedin.com/in/tsgiannis/");
            companyLabel.Font = new Font("Arial", 10, FontStyle.Bold);
            companyLabel.ForeColor = Color.Blue;
            companyLabel.AutoSize = true;
            companyLabel.Location = new Point(logo.Left, logo.Bottom + 5);
            // Add the LinkClicked event handler
            companyLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(companyLabel_LinkClicked);
            this.Controls.Add(companyLabel);
        }

        private void DashboardForm_Paint(object sender, PaintEventArgs e)
        {
            // Create a darker gradient background
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            using (LinearGradientBrush brush = new LinearGradientBrush(rect,
                Color.FromArgb(25, 25, 112), // MidnightBlue
                Color.FromArgb(72, 61, 139), // DarkSlateBlue
                LinearGradientMode.ForwardDiagonal))
            {
                e.Graphics.FillRectangle(brush, rect);
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton.Name == "btnQuote")
                Process.Start("winword.exe");
            else
                MessageBox.Show($"You clicked the {clickedButton.Text} button!");
        }
    }
}