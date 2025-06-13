namespace ZippyGoIfood
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
        private System.Windows.Forms.Button btnCapturar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
            btnCapturar = new Button();
            ((System.ComponentModel.ISupportInitialize)webView21).BeginInit();
            SuspendLayout();
            // 
            // webView21
            // 
            webView21.AllowExternalDrop = true;
            webView21.CreationProperties = null;
            webView21.DefaultBackgroundColor = Color.White;
            webView21.Dock = DockStyle.Fill;
            webView21.Location = new Point(0, 36);
            webView21.Name = "webView21";
            webView21.Size = new Size(1000, 704);
            webView21.TabIndex = 0;
            webView21.ZoomFactor = 1D;
            // 
            // btnCapturar
            // 
            btnCapturar.Dock = DockStyle.Top;
            btnCapturar.Location = new Point(0, 0);
            btnCapturar.Name = "btnCapturar";
            btnCapturar.Size = new Size(1000, 36);
            btnCapturar.TabIndex = 1;
            btnCapturar.Text = "Capturar Pedido";
            btnCapturar.Click += btnCapturar_Click;
            // 
            // Form1
            // 
            ClientSize = new Size(1000, 740);
            Controls.Add(webView21);
            Controls.Add(btnCapturar);
            Name = "Form1";
            Text = "ZippyGo - Captura de Pedidos iFood";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)webView21).EndInit();
            ResumeLayout(false);
        }
    }
}
