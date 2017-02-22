using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouTubeGrabber
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
}
