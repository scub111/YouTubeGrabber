using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace YouTubeGrabber
{
    /// <summary>
    /// Основной класс глобальных переменных, которых можно сохранить в XML-файд
    /// </summary>
    public class VarXml
    {
        /// <summary>
        /// Конструктор <see cref="GlobalVarBase"/> class.
        /// </summary>
        public VarXml()
        {
            FileXml = "Config.xml";
            Init();
        }

        /// <summary>
        /// Конструктор <see cref="GlobalVarBase"/> class.
        /// </summary>
        public VarXml(string strFileXml)
        {
            FileXml = strFileXml;
            Init();
        }

        void Init()
        {
            FilePath = string.Format("{0}\\{1}", Path.GetDirectoryName(Application.ExecutablePath), FileXml);
            AnalyseUri = "https://www.youtube.com/watch?v=c1NCzfo2_jo&list=PL08903FB7ACA1C2FB&index=132";
            ThreadCount = 4;
            DownloadPath = @"C:\Downloads\";
        }

        /// <summary>
        /// Название файла, куда будет сохняться данные.
        /// </summary>
        [XmlIgnore]
        string FileXml;

        /// <summary>
        /// Путь к файлу.
        /// </summary>
        [XmlIgnore]
        public string FilePath;

        /// <summary>
        /// Анализируемая ссылка.
        /// </summary>
        public string AnalyseUri;

        /// <summary>
        /// Путь загрузки записей.
        /// </summary>
        public string DownloadPath;

        /// <summary>
        /// Количество потоков в пуле.
        /// </summary>
        public int ThreadCount;

        /// <summary>
        /// Сохранить данные в XML-файл.гз
        /// </summary>
        public void SaveToXML()
        {
            XmlSerializer xmlSer = new XmlSerializer(typeof(VarXml));
            TextWriter textWriter = new StreamWriter(FilePath);
            xmlSer.Serialize(textWriter, this);
            textWriter.Close();
        }

        /// <summary>
        /// Загрузить данные из XML-файла.
        /// </summary>
        /// <returns></returns>
        public void LoadFromXML()
        {
            if (File.Exists(FilePath))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(VarXml));
                TextReader textReader = new StreamReader(FilePath);
                VarXml obj = (VarXml)deserializer.Deserialize(textReader);
                textReader.Close();

                AnalyseUri = obj.AnalyseUri;
                DownloadPath = obj.DownloadPath;
                ThreadCount = obj.ThreadCount;
            }
        }
    }

    public class GlobalDefault
    {
        /// <summary>
        /// Версия программы.
        /// </summary>
        public string Version;

        /// <summary>
        /// Переменные из файла настроек.
        /// </summary>
        public VarXml varXml;

        /// <summary>
        /// Инициализация переменных.
        /// </summary>
        public void Init()
        {
            Version = "v1.0.4";

            string fileName = Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location);
            varXml = new VarXml(string.Format("{0}.xml", fileName));
            varXml.LoadFromXML();
            //varXml.SaveToXML();
        }
    }

    public class Global
    {
        private static GlobalDefault defaultInstance = new GlobalDefault();
        public static GlobalDefault Default { get { return defaultInstance; } }
    }
}
