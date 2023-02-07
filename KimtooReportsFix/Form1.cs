
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KimtooReportsFix
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {


            //Populate random data 
            var random = new Random();
            for (int i = 0; i < 100; i++)
            {
                var productName = "Product" + (i + 1);
                var productPrice = random.Next(100, 5000);
                dataGridView1.Rows.Add(new object[]
                {
                    productName,
                    productPrice.ToString("C2")  // CURRENCY FORMAT. Right Aligned from the UI
                });
            }

        }

        void GenerateReport()
        {
            ktReport1.Clear();

            ktReport1.AddString("<h2>Product List</h2>");
            ktReport1.AddHorizontalRule();
            ktReport1.AddDatagridView(dataGridView1);
        }

        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateReport();
            saveFileDialog1.ShowDialog();

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Generate report
            // This relies on the webBrowser control, It isn't effective when rendreing tables.
            // I will be pushing an update which utilizes webView

            GenerateReport();

            ktReport1.ShowPrintPreviewDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

            // I recommend this option 
            ktReport1.SavePDF(saveFileDialog1.FileName);
            Process.Start(saveFileDialog1.FileName);
        }

        private void openInBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateReport();
            var code = ktReport1.GetHTML();
            File.WriteAllText("report.html", code);
            var uri = new Uri(new FileInfo("report.html").FullName);
            Process.Start(uri.AbsoluteUri);

        }
        bool init = true;
        private void webView21_CoreWebView2InitializationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs e)
        {
            if (init)
            {
                init = false;
                return;
            }
            openInBrowserToolStripMenuItem.Enabled = true;
        }

        private void webView21_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {

            webView21.CoreWebView2.ShowPrintUI();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

            GenerateReport();
            tabControl1.SelectedIndex = 1;
            var code = ktReport1.GetHTML();
            File.WriteAllText("report.html", code);
            var uri = new Uri(new FileInfo("report.html").FullName);
            webView21.CoreWebView2.Navigate(uri.AbsoluteUri);
        }
    }
}
