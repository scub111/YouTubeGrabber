using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Base.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Usable;
using YoutubeExtractor;

namespace YouTubeGrabber
{
    public partial class MainForm : XtraForm
    {
        /// <summary>
        /// Приоритет загрузки.
        /// </summary>
        public enum Priority
        {

            High,   //Высокий.
            Normal, //Нормальный.
            Low,    //Низкий.
            None    //Не загружать.
        }

        /// <summary>
        /// Класс записей на канале.
        /// </summary>
        public class YouTubeRecord
        {
            public YouTubeRecord()
            {
                Priority = Priority.Normal;
            }

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
            /// Приоритет.
            /// </summary>
            public Priority Priority { get; set; }

            /// <summary>
            /// Процент загрузки.
            /// </summary>
            public double ProgressPercentage { get; set; }
        }

        public class Records : Collection<YouTubeRecord>
        {

        }


        public class VideoDownloaderEx : VideoDownloader
        {
            public VideoDownloaderEx(VideoInfo video, string savePath, object tag = null) : base(video, savePath)
            {
                Tag = tag;
            }
            
            public object Tag { get; private set; }
        }

        public MainForm()
        {
            InitializeComponent();
            workerAnalyse = new BackgroundWorker();
            workerAnalyse.WorkerReportsProgress = true;
            workerAnalyse.DoWork += WorkerAnalyse_DoWork;
            workerAnalyse.ProgressChanged += WorkerAnalyse_ProgressChanged;
            html = "No data";
            records = new Records();
            threadPool = new FixedThreadPool(4);
            threadPool.Finished += ThreadPool_Finished;
            Downloading = false;
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
        private Records records;

        /// <summary>
        /// Пул поток для загрузки записей.
        /// </summary>
        private FixedThreadPool threadPool;

        /// <summary>
        /// Загружаются ли сейчас записи.
        /// </summary>
        private bool Downloading;

        private void MainForm_Load(object sender, EventArgs e)
        {
            SplashScreenManager.CloseForm(false);
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

        static void DeleteFile(string file)
        {
            if (System.IO.File.Exists(file)) System.IO.File.Delete(file);
        }

        /// <summary>
        /// Переименование файла.
        /// </summary>
        static void RenameFile(string oldFile, string newFile)
        {
            DeleteFile(newFile);
            System.IO.File.Move(oldFile, newFile);
        }

        /// <summary>
        /// Загрузка записи в определенную директорию.
        /// </summary>
        /// <param name="url">Адрес записи</param>
        /// <param name="path">Директория сохранения</param>
        private void DownloadRecord(string link, string path, string name, YouTubeRecord record)
        {
            string tempFile = "";
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(link, false);
                VideoInfo video = videoInfos.First();

                string finalFile = Path.Combine(path, RemoveIllegalPathCharacters(name) + video.VideoExtension);

                if (!System.IO.File.Exists(finalFile))
                {
                    tempFile = Path.Combine(path, RemoveIllegalPathCharacters(name) + ".tmp");
                    VideoDownloaderEx videoDownloader = new VideoDownloaderEx(video, tempFile, record);
                    videoDownloader.DownloadProgressChanged += VideoDownloader_DownloadProgressChanged;
                    videoDownloader.Execute();
                    RenameFile(tempFile, finalFile);
                }
                record.ProgressPercentage = 100;
            }
            catch
            {
                if (!string.IsNullOrEmpty(tempFile))
                    DeleteFile(tempFile);
            }
        }

        private void VideoDownloader_DownloadProgressChanged(object sender, ProgressEventArgs e)
        {
            VideoDownloaderEx videoEx = sender as VideoDownloaderEx;
            if (videoEx != null)
                ((YouTubeRecord)videoEx.Tag).ProgressPercentage = e.ProgressPercentage;
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
                slblCaption.Text = "Analysing...";
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
            Downloading = !Downloading;

            if (Downloading)
            {
                Download();
                btnDownload.Text = "Stop";
            }
            else
            {
                threadPool.Stop();
                btnDownload.Text = "Download";
            }
        }

        /// <summary>
        /// Загружка выбранных записей.
        /// </summary>
        private void Download()
        {
            Uri uri = new Uri(tdInputUri.Text);

            IEnumerable<YouTubeRecord> downloads = records.Where(item => item.Priority != Priority.None);

            int log = (int)Math.Log10(records.Count()) + 1;
            string format = string.Format("D{0}", log);

            foreach (YouTubeRecord record in downloads)
            {
                Action act = new Action(() =>
                DownloadRecord(string.Format("{0}{1}", uri.Authority, record.Reference),
                teDownloadPath.Text,
                string.Format("{0}. {1}", record.Index.ToString(format), record.Title),
                record));

                switch (record.Priority)
                {
                    case Priority.High:
                        threadPool.Execute(act, TaskPriorityEx.HIGH);
                        break;

                    case Priority.Normal:
                        threadPool.Execute(act, TaskPriorityEx.NORMAL);
                        break;

                    case Priority.Low:
                        threadPool.Execute(act, TaskPriorityEx.LOW);
                        break;
                }
            }
        }

        private void tmrInterfaceUpdate_Tick(object sender, EventArgs e)
        {
            gridBase.RefreshDataSource();

            if (threadPool.TaskCount > 0)
            {
                int count = threadPool.TaskCount;
                int executed = threadPool.ExecutedCount;
                double per = 0;
                if (count > 0)
                    per = executed / (double)count * 100;
                sprbProgress.Value = (int)per;
                slblCaption.Text = string.Format("{0} / {1}", executed, count);
            }
        }

        private void viewBase_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            
            if (e.MenuType == GridMenuType.Row)
            {
                GridViewMenu menu = e.Menu;              

                //Adding new items 
                menu.Items.Add(new DXMenuCheckItem("High", false, null, new EventHandler((obj, arg) => ChangePriority(Priority.High))));
                menu.Items.Add(new DXMenuCheckItem("Normal", false, null, new EventHandler((obj, arg) => ChangePriority(Priority.Normal))));
                menu.Items.Add(new DXMenuCheckItem("Low", false, null, new EventHandler((obj, arg) => ChangePriority(Priority.Low))));
                menu.Items.Add(new DXMenuCheckItem("None", false, null, new EventHandler((obj, arg) => ChangePriority(Priority.None))));
            }
        }

        private void ChangePriority(Priority priority)
        {
            for (int i = 0; i < viewBase.SelectedRowsCount; i++)
            {
                int row = (viewBase.GetSelectedRows()[i]);
                YouTubeRecord obj = viewBase.GetRow(row) as YouTubeRecord;
                if (obj != null)
                    obj.Priority = priority;
            }
            gridBase.RefreshDataSource();
        }

        private void ThreadPool_Finished(object sender, EventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
                tmrInterfaceUpdate_Tick(this, EventArgs.Empty);
                btnDownload.Text = "Download";
                Downloading = false;
            }));
        }
    }
}
