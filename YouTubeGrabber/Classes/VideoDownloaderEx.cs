using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExtractor;

namespace YouTubeGrabber
{
    /// <summary>
    /// Модицированный класс VideoDownloader с дополнительной ссылкой на объект.
    /// </summary>
    public class VideoDownloaderEx : VideoDownloader
    {
        public VideoDownloaderEx(VideoInfo video, string savePath, object tag = null) : base(video, savePath)
        {
            Tag = tag;
        }

        public object Tag { get; private set; }
    }
}
