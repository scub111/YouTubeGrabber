using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Usable;
using YoutubeExtractor;

namespace YouTubeGrabber
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Класс записей на канале.
        /// </summary>
        public class YouTubeRecord
        {
            /// <summary>
            /// Индекс.
            /// </summary>
            public int Index { get; set; }

            /// <summary>
            /// Заголовок.
            /// </summary>
            public string Title { get; set; }

            /// <summary>
            /// Ссылка.
            /// </summary>
            public string Reference { get; set; }

            /// <summary>
            /// Процент загрузки.
            /// </summary>
            public double ProgressPercentage { get; set; }
        }

        public MainForm()
        {
            InitializeComponent();
            workerAnalyse = new BackgroundWorker();
            workerAnalyse.WorkerReportsProgress = true;
            workerAnalyse.DoWork += WorkerAnalyse_DoWork;
            workerAnalyse.ProgressChanged += WorkerAnalyse_ProgressChanged;
            html = "No data";
            records = new Collection<YouTubeRecord>();
            threadPool = new FixedThreadPool(4);
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

        /// <summary>
        /// Список записей.
        /// </summary>
        private Collection<YouTubeRecord> records;

        /// <summary>
        /// Пул поток для загрузки записей.
        /// </summary>
        private FixedThreadPool threadPool;

        private void MainForm_Load(object sender, EventArgs e)
        {
            gridBase.DataSource = records;
        }

        /// <summary>
        /// Удаление некорретных символов.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string RemoveIllegalPathCharacters(string path)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(path, "");
        }

        /// <summary>
        /// Загрузка записи в определенную директорию.
        /// </summary>
        /// <param name="url">Адрес записи</param>
        /// <param name="path">Директория сохранения</param>
        private void DownloadRecord(string link, string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(link, false);
            VideoInfo video = videoInfos.First();

            var videoDownloader = new VideoDownloader(video,
                    Path.Combine(path, RemoveIllegalPathCharacters(video.Title) + video.VideoExtension));

            videoDownloader.DownloadProgressChanged += VideoDownloader_DownloadProgressChanged;
            videoDownloader.Execute();
        }

        private void VideoDownloader_DownloadProgressChanged(object sender, ProgressEventArgs e)
        {
            if (records.Count > 0)
                records[0].ProgressPercentage = e.ProgressPercentage;
        }

        private void WorkerAnalyse_DoWork(object sender, DoWorkEventArgs e)
        {
            workerAnalyse.ReportProgress(0);
            try
            {
                records.Clear();
                Uri uri = new Uri(tdInputUri.Text);
                html = new WebClient().DownloadString(uri);

                /*
                string path = @"YouTube-test.txt";

                if (File.Exists(path))
                    html = File.ReadAllText(path);
                */
                workerAnalyse.ReportProgress(1);

                HtmlParser parser = new HtmlParser();
                IHtmlDocument angle = parser.Parse(html);

                //IHtmlCollection<IElement> list = angle.All.Where(i => i.LocalName == "li");
                string[] strArr = new string[1];
                int index = 0;
                string reference = "";
                IHtmlCollection<IElement> list = angle.QuerySelectorAll("li.yt-uix-scroller-scroll-unit");
                foreach (IElement element in list)
                {
                    IHtmlCollection<IElement> indexes = element.QuerySelectorAll("span.index");
                    if (indexes.Count() > 0)
                        strArr = indexes[0].TextContent.Split('\n');

                    int result = 1;
                    if (strArr.Length == 3 && int.TryParse(strArr[1], out result))
                        index = result;

                    IHtmlCollection<IElement> references = element.QuerySelectorAll("a");
                    if (references.Count() > 0)
                        reference = references[0].GetAttribute("href");

                    YouTubeRecord record = new YouTubeRecord()
                    {
                        Index = index,
                        Title = element.GetAttribute("data-video-title"),
                        Reference = reference
                    };
                    //records.Add(new YouTubeRecord(0, ))
                    //Console.WriteLine(element.GetAttribute("href"));
                    records.Add(record);
                }

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
            else if (e.ProgressPercentage == 2)
            {
                sprbProgress.Value = 100;
                gridBase.RefreshDataSource();
                TimeSpan diff = DateTime.Now - t0;
                slblCaption.Text = string.Format("{0:0} ms", diff.TotalMilliseconds);
            }
            else if (e.ProgressPercentage == -1)
            {
                memoEdit1.Text = html;
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

        private void btnDownload_Click(object sender, EventArgs e)
        {
            Uri uri = new Uri(tdInputUri.Text);

            /*if (records.Count > 0)
                DownloadRecord(string.Format("{0}{1}", uri.Authority, records[0].Reference), teDownloadPath.Text);
            */
            Action act = new Action(() => DownloadRecord(string.Format("{0}{1}", uri.Authority, records[0].Reference), teDownloadPath.Text));

            threadPool.Execute(act, TaskPriorityEx.NORMAL);

        }

        private void tmrInterfaceUpdate_Tick(object sender, EventArgs e)
        {
            gridBase.RefreshDataSource();
        }
    }
}
