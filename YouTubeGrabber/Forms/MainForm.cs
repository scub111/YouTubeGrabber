using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using AngleSharp.Parser.Html;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using Usable;
using YoutubeExtractor;

namespace YouTubeGrabber.Forms
{
    public partial class MainForm : XtraForm
    {
        public MainForm()
        {
            Global.Default.Init();
            InitializeComponent();
            _workerAnalyse = new BackgroundWorker {WorkerReportsProgress = true};
            _workerAnalyse.DoWork += WorkerAnalyse_DoWork;
            _workerAnalyse.ProgressChanged += WorkerAnalyse_ProgressChanged;
            _html = "No data";
            _records = new Collection<YouTubeRecord>();
            _events = new Collection<EventLog>();
            _downloading = false;
        }

        /// <summary>
        /// Фоновый поток.
        /// </summary>
        private readonly BackgroundWorker _workerAnalyse;

        /// <summary>
        /// HTML-код страницы.
        /// </summary>
        private string _html;

        /// <summary>
        /// Начальное время выполнения задачи.
        /// </summary>
        private DateTime _t0;

        /// <summary>
        /// Список записей.
        /// </summary>
        private readonly Collection<YouTubeRecord> _records;

        /// <summary>
        /// Список событий.
        /// </summary>
        private readonly Collection<EventLog> _events;

        /// <summary>
        /// Пул поток для загрузки записей.
        /// </summary>
        private FixedThreadPool _threadPool;

        /// <summary>
        /// Загружаются ли сейчас записи.
        /// </summary>
        private bool _downloading;

        private void MainForm_Load(object sender, EventArgs e)
        {
            Text += $" {Global.Default.Version}";
            teAnalyseUri.Text = Global.Default.varXml.AnalyseUri;
            teDownloadPath.Text = Global.Default.varXml.DownloadPath;
            seThreadCount.Value = Global.Default.varXml.ThreadCount;
            SplashScreenManager.CloseForm(false);
            gridBase.DataSource = _records;
            gridEventLog.DataSource = _events;
            PublishEvent("Program was started normaly.");
        }

        /// <summary>
        /// Удаление некорретных символов.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string ReplaceIllegalPathCharacters(string path)
        {
            var regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var r = new Regex($"[{Regex.Escape(regexSearch)}]");
            return r.Replace(path, ",");
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
            _events.Add(new EventLog(eventLog));
            gridEventLog.RefreshDataSource();
        }

        /// <summary>
        /// Загрузка записи в определенную директорию.
        /// </summary>
        private void DownloadRecord(string link, string path, string name, YouTubeRecord record)
        {
            var tempFile = "";
            name = name.Replace('/', '-');
            try
            {
                BeginInvoke(new Action(() =>
                {
                    PublishEvent($"[{name}] - try to download...");
                }));

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var videoInfos = DownloadUrlResolver.GetDownloadUrls(link, false);
                var video = videoInfos.First();

                var finalFile = Path.Combine(path, ReplaceIllegalPathCharacters(name) + video.VideoExtension);

                if (!File.Exists(finalFile))
                {
                    tempFile = Path.Combine(path, ReplaceIllegalPathCharacters(name) + ".tmp");
                    var videoDownloader = new VideoDownloaderEx(video, tempFile, record);
                    videoDownloader.DownloadProgressChanged += VideoDownloader_DownloadProgressChanged;
                    videoDownloader.Execute();
                    RenameFile(tempFile, finalFile);
                }
                record.ProgressPercentage = 100;

                BeginInvoke(new Action(() =>
                {
                    PublishEvent($"[{name}] - was downloaded successfully");
                }));
            }
            catch(Exception ex)
            {
                BeginInvoke(new Action(() =>
                {
                    PublishEvent($"{ex.Source} - {ex.Message}");
                }));

                if (!string.IsNullOrEmpty(tempFile))
                    DeleteFile(tempFile);
            }
        }

        private void VideoDownloader_DownloadProgressChanged(object sender, ProgressEventArgs e)
        {
            if (sender is VideoDownloaderEx videoEx)
                ((YouTubeRecord)videoEx.Tag).ProgressPercentage = e.ProgressPercentage;
        }

        private void WorkerAnalyse_DoWork(object sender, DoWorkEventArgs e)
        {
            _workerAnalyse.ReportProgress(0);
            try
            {
                _records.Clear();
                var uri = new Uri(teAnalyseUri.Text);
                var webClient = new WebClient {Encoding = Encoding.UTF8};
                _html = webClient.DownloadString(uri);
                _workerAnalyse.ReportProgress(1);

                var parser = new HtmlParser();
                var angle = parser.Parse(_html);

                //IHtmlCollection<IElement> list = angle.All.Where(i => i.LocalName == "li");
                var strArr = new string[1];
                var index = 1;
                var reference = "";
                var list = angle.QuerySelectorAll("li.yt-uix-scroller-scroll-unit");
                foreach (var element in list)
                {
                    var indexes = element.QuerySelectorAll("span.index");
                    if (indexes != null && indexes.Any())
                        strArr = indexes[0].TextContent.Split('\n');

                    if (strArr.Length == 3 && int.TryParse(strArr[1], out var result))
                        index = result;

                    var references = element.QuerySelectorAll("a");
                    if (references != null && references.Any())
                        reference = references[0].GetAttribute("href");

                    var record = new YouTubeRecord()
                    {
                        Index = index,
                        Title = element.GetAttribute("data-video-title"),
                        Reference = reference
                    };
                    _records.Add(record);
                }

                _workerAnalyse.ReportProgress(2);
            }
            catch (Exception ex)
            {
                _html = $"{ex.Source} - {ex.Message}";
                _workerAnalyse.ReportProgress(-1);
            }
        }

        private void WorkerAnalyse_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
            {
                sprbProgress.Value = 10;
                slblCaption.Text = @"Analysing...";
            }
            else if (e.ProgressPercentage == 1)
            {
                memoEdit1.Text = _html;
                sprbProgress.Value = 80;
            }
            else if (e.ProgressPercentage == 2)
            {
                sprbProgress.Value = 100;
                gridBase.RefreshDataSource();
                var diff = DateTime.Now - _t0;
                slblCaption.Text = $@"{diff.TotalMilliseconds:0} ms";
                PublishEvent("Analysis of html-code was completed successfully.");
            }
            else if (e.ProgressPercentage == -1)
            {
                memoEdit1.Text = _html;
                var diff = DateTime.Now - _t0;
                slblCaption.Text = $@"{diff.TotalMilliseconds:0} ms";
            }
        }

        private void btnAnalyse_Click(object sender, EventArgs e)
        {
            if (!_workerAnalyse.IsBusy)
            {
                _t0 = DateTime.Now;
                _workerAnalyse.RunWorkerAsync();
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            _downloading = !_downloading;

            if (_downloading)
            {
                Download();
                btnDownload.Text = "Stop";
            }
            else
            {
                _threadPool.Stop();
                btnDownload.Text = "Download";
            }
        }

        /// <summary>
        /// Загружка выбранных записей.
        /// </summary>
        private void Download()
        {
            var infos = new[] { teAnalyseUri.Text };

            if (!Directory.Exists(teDownloadPath.Text))
                Directory.CreateDirectory(teDownloadPath.Text);

            File.WriteAllLines(Path.Combine(teDownloadPath.Text, "_Info.txt"), infos);

            _threadPool = new FixedThreadPool(Global.Default.varXml.ThreadCount);
            _threadPool.Finished += ThreadPool_Finished;

            foreach (var record in _records)
                record.ProgressPercentage = 0;

            var uri = new Uri(teAnalyseUri.Text);

            var downloads = _records.Where(item => item.Priority != Priority.None);

            var log = (int)Math.Log10(_records.Count) + 1;
            var format = $"D{log}";

            foreach (var record in downloads)
            {
                var act = new Action(() =>
                DownloadRecord($"{uri.Authority}{record.Reference}",
                teDownloadPath.Text,
                    $"{record.Index.ToString(format)}. {record.Title}",
                record));

                switch (record.Priority)
                {
                    case Priority.High:
                        _threadPool.Execute(act, TaskPriorityEx.HIGH);
                        break;

                    case Priority.Normal:
                        _threadPool.Execute(act, TaskPriorityEx.NORMAL);
                        break;

                    case Priority.Low:
                        _threadPool.Execute(act);
                        break;
                }
            }
        }

        private void tmrInterfaceUpdate_Tick(object sender, EventArgs e)
        {
            gridBase.RefreshDataSource();

            if (_threadPool != null && _threadPool.TaskCount > 0)
            {
                var count = _threadPool.TaskCount;
                var executed = _threadPool.ExecutedCount;
                double per = 0;
                if (count > 0)
                    per = executed / (double)count * 100;
                sprbProgress.Value = (int)per;
                slblCaption.Text = $"{executed} / {count}";
            }
        }

        private void viewBase_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {            
            if (e.MenuType == GridMenuType.Row)
            {
                var menu = e.Menu;              

                //Adding new items 
                menu.Items.Add(new DXMenuCheckItem("High", false, null, (obj, arg) => ChangePriority(Priority.High)));
                menu.Items.Add(new DXMenuCheckItem("Normal", false, null, (obj, arg) => ChangePriority(Priority.Normal)));
                menu.Items.Add(new DXMenuCheckItem("Low", false, null, (obj, arg) => ChangePriority(Priority.Low)));
                menu.Items.Add(new DXMenuCheckItem("None", false, null, (obj, arg) => ChangePriority(Priority.None)));
            }
        }

        private void ChangePriority(Priority priority)
        {
            for (var i = 0; i < viewBase.SelectedRowsCount; i++)
            {
                var row = (viewBase.GetSelectedRows()[i]);
                if (viewBase.GetRow(row) is YouTubeRecord obj)
                    obj.Priority = priority;
            }
            gridBase.RefreshDataSource();
        }

        private void ThreadPool_Finished(object sender, EventArgs e)
        {
            BeginInvoke(new Action(() =>
            {
                tmrInterfaceUpdate_Tick(this, EventArgs.Empty);
                btnDownload.Text = "Download";
                _downloading = false;

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
