using System.Collections.Generic;
using System.Linq;
using SvnTimeService.Server.Contracts;

namespace SvnTimeService.Server.Core
{
    public class SvnTimeManager
    {
        public const string FileName = "SvnTime.csv";

        public IImportService ImportService { get; set; }
        
        public List<SvnLogItem> LogItems { get; } = new List<SvnLogItem>();

        private static SvnTimeManager _instance = new SvnTimeManager(FileName);

        private SvnTimeManager(string fileName)
        {
            ImportService = new FileImportService(fileName);
            ImportLog();
        }

        public static SvnTimeManager GetInstance()
        {
            return _instance;
        }

        public void Add(string importLine)
        {
            LogItems.Add(new SvnLogItem(importLine));
        }

        private void ImportLog()
        {
            string[] logLines = ImportService.Load();
            foreach (var logLine in logLines)
            {
                Add(logLine);
            }
        }

        public double GetUserDuration(string user)
        {
            double totalDuration = 0;
            foreach (var logItem in LogItems)
            {
                if (logItem.Author == user)
                {
                    totalDuration += logItem.Duration;
                }
            }

            return totalDuration;
        }

        public double GetAverageDuration()
        {
            double totalDuration = 0;
            foreach (var logItem in LogItems)
            {
                totalDuration += logItem.Duration;
            }

            return totalDuration / LogItems.Count;
        }

        public List<SvnLogItem> GetUserDiary(string user)
        {
            //return LogItems.Where(logItem => logItem.Author == user).ToList();

            List<SvnLogItem> itemsWhereUserIsAuthor = new List<SvnLogItem>();
            foreach (var logItem in LogItems)
            {
                if (logItem.Author == user)
                {
                    itemsWhereUserIsAuthor.Add(logItem);
                }
            }

            return itemsWhereUserIsAuthor;
        }
    }
}