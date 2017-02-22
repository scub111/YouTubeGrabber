using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouTubeGrabber
{
    /// <summary>
    /// Класс событий, которые появляются в ходе выполнения программы.
    /// </summary>
    public class EventLog
    {
        public EventLog(string eventLog)
        {
            Time = DateTime.Now;
            Event = eventLog;
        }
        /// <summary>
        /// Время события.
        /// </summary>
        public DateTime Time { get; private set; }

        /// <summary>
        /// Событие.
        /// </summary>
        public string Event { get; private set; }
    }
}
