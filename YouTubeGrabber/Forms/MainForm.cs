using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
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
    public partial class MainForm : XtraForm
    {
        public MainForm()
        {
            Global.Default.Init();
            InitializeComponent();
            workerAnalyse = new BackgroundWorker();
            workerAnalyse.WorkerReportsProgress = true;
            workerAnalyse.DoWork += WorkerAnalyse_DoWork;
            workerAnalyse.ProgressChanged += WorkerAnalyse_ProgressChanged;
            html = "No data";
            records = new Collection<YouTubeRecord>();
            events = new Collection<EventLog>();
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
        private Collection<YouTubeRecord> records;

        /// <summary>
        /// Список событий.
        /// </summary>
        private Collection<EventLog> events;

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
            Text += string.Format(" {0}", Global.Default.Version);
            teAnalyseUri.Text = Global.Default.varXml.AnalyseUri;
            teDownloadPath.Text = Global.Default.varXml.DownloadPath;
            seThreadCount.Value = Global.Default.varXml.ThreadCount;
            SplashScreenManager.CloseForm(false);
            gridBase.DataSource = records;
            gridEventLog.DataSource = events;
            PublishEvent("Program was started normaly.");
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
            if (File.Exists(file)) File.Delete(file);
        }

        /// <summary>
        /// Переименование файла.
        /// </summary>
        static void RenameFile(string oldFile, string newFile)
        {
            DeleteFile(newFile);
            File.Move(oldFile, newFile);
        }

        private void PublishEvent(string eventLog)
        {
            events.Add(new EventLog(eventLog));
            gridEventLog.RefreshDataSource();
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
                this.BeginInvoke(new Action(() =>
                {
                    PublishEvent(string.Format("[{0}] - try to download...", name));
                }));

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(link, false);
                VideoInfo video = videoInfos.First();

                string finalFile = Path.Combine(path, RemoveIllegalPathCharacters(name) + video.VideoExtension);

                if (!File.Exists(finalFile))
                {
                    tempFile = Path.Combine(path, RemoveIllegalPathCharacters(name) + ".tmp");
                    VideoDownloaderEx videoDownloader = new VideoDownloaderEx(video, tempFile, record);
                    videoDownloader.DownloadProgressChanged += VideoDownloader_DownloadProgressChanged;
                    videoDownloader.Execute();
                    RenameFile(tempFile, finalFile);
                }
                record.ProgressPercentage = 100;

                this.BeginInvoke(new Action(() =>
                {
                    PublishEvent(string.Format("[{0}] - was downloaded successfully", name));
                }));
            }
            catch(Exception ex)
            {
                this.BeginInvoke(new Action(() =>
                {
                    PublishEvent(string.Format("{0} - {1}", ex.Source, ex.Message));
                }));

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
                Uri uri = new Uri(teAnalyseUri.Text);
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
                PublishEvent("Analysis of html-code was completed successfully.");
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
            string[] infos = new string[] { Global.Default.varXml.AnalyseUri };

            File.WriteAllLines(Path.Combine(Global.Default.varXml.DownloadPath, "_Info.txt"), infos);

            threadPool = new FixedThreadPool(Global.Default.varXml.ThreadCount);
            threadPool.Finished += ThreadPool_Finished;

            foreach (YouTubeRecord record in records)
                record.ProgressPercentage = 0;

            Uri uri = new Uri(teAnalyseUri.Text);

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

            if (threadPool != null && threadPool.TaskCount > 0)
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

                PublishEvent("Finish of dowloading attempts all records.");
            }));
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Global.Default.varXml.AnalyseUri = teAnalyseUri.Text;
            Global.Default.varXml.DownloadPath = teDownloadPath.Text;
            Global.Default.varXml.ThreadCount = (int)seThreadCount.Value;
            Global.Default.varXml.SaveToXML();
        }

        private void seThreadCount_EditValueChanged(object sender, EventArgs e)
        {
            Global.Default.varXml.ThreadCount = (int)seThreadCount.Value;
        }
    }
}
