using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YouTubeGrabber
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            workerAnalyse = new BackgroundWorker();
            workerAnalyse.WorkerReportsProgress = true;
            workerAnalyse.DoWork += WorkerAnalyse_DoWork;
            workerAnalyse.ProgressChanged += WorkerAnalyse_ProgressChanged;
            html = "No data";
        }

        /// <summary>
        /// Фоновый поток.
        /// </summary>
        private BackgroundWorker workerAnalyse;

        /// <summary>
        /// HTML-код страницы.
        /// </summary>
        private string html;

        /// <summary>
        /// Начальное время выполнения задачи.
        /// </summary>
        private DateTime t0;

        private void WorkerAnalyse_DoWork(object sender, DoWorkEventArgs e)
        {
            workerAnalyse.ReportProgress(0);
            try
            {
                //Uri uri = new Uri(tdInputUrl.Text);
                //html = new WebClient().DownloadString(uri);

                string path = @"YouTube-test.txt";

                if (File.Exists(path))
                    html = File.ReadAllText(path);
                workerAnalyse.ReportProgress(1);

                HtmlParser parser = new HtmlParser();
                IHtmlDocument angle = parser.Parse(html);

                var list = angle.All.Where(i => i.LocalName == "li");
                /*foreach (IElement element in angle.QuerySelectorAll("h3.r a"))
                    Console.WriteLine(element.GetAttribute("href"));
                    */
                workerAnalyse.ReportProgress(2);
            }
            catch (Exception ex)
            {
                html = string.Format("{0} - {1}", ex.Source, ex.Message);
                workerAnalyse.ReportProgress(-1);
            }
        }

        private void WorkerAnalyse_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
            {
                sprbProgress.Value = 10;
                slblCaption.Text = "Running...";
            }
            else if (e.ProgressPercentage == 1)
            {
                memoEdit1.Text = html;
                sprbProgress.Value = 80;
            }
            else
            {
                TimeSpan diff = DateTime.Now - t0;
                slblCaption.Text = string.Format("{0:0} ms", diff.TotalMilliseconds);
            }
        }

        private void btnAnalyse_Click(object sender, EventArgs e)
        {
            if (!workerAnalyse.IsBusy)
            {
                t0 = DateTime.Now;
                workerAnalyse.RunWorkerAsync();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }
    }
}
