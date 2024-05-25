using System;
using System.Collections.Generic;

namespace SvnTimeService.Server.Core
{
    public class SvnTimeRequestHandler
    {
        private string _request;

        public SvnTimeRequestHandler(string request)
        {
            _request = request;
        }

        public string GetResponse()
        {
            try
            {
                string[] splitString = _request.Split(' ');
                //2. parameter true ist für ignoreCase (Groß- und Kleinschreibung ignorieren)
                bool commandFound = Enum.TryParse(splitString[0], true, out Command command);
                if (!commandFound)
                {
                    return "Invalid command";
                }
                switch (command)
                {
                    case Command.GetList:
                        return GetList();
                    case Command.GetDuration:
                        return GetDuration(user: splitString[1]);
                    case Command.GetUserDiary:
                        return GetUserDiary(user: splitString[1]);
                    default:
                        return "Invalid command";
                }
            }
            catch (Exception)
            {
                return "Invalid command";
            }
        }

        private string GetList()
        {
            List<SvnLogItem> logItems = SvnTimeManager.GetInstance().LogItems;
            //string.Join erstellt ein String aus einer Liste und trennt die Elemente der Liste durch "\n\r" in dem Fall
            // \n bedeutet neue Zeile und \r Wagenrücklauf (damit die Zeile wieder von ganz links anfängt)
            string itemsSeparatedByNewLine = string.Join("\n\r", logItems);
            //"Letzte Zeile Anzahl der Einträge"
            itemsSeparatedByNewLine += $"\n\rCount: {logItems.Count}";
            return itemsSeparatedByNewLine;
        }

        private string GetDuration(string user)
        {
            SvnTimeManager manager = SvnTimeManager.GetInstance();
            double userDuration = manager.GetUserDuration(user);
            double averageDuration = manager.GetAverageDuration();
            return $"User: {userDuration} | Average: {averageDuration}";
        }
        
        private string GetUserDiary(string user)
        {
            SvnTimeManager manager = SvnTimeManager.GetInstance();
            List<SvnLogItem> logItems = manager.GetUserDiary(user);
            //string.Join erstellt ein String aus einer Liste und trennt die Elemente der Liste durch "\n\r" in dem Fall
            // \n bedeutet neue Zeile und \r Wagenrücklauf (damit die Zeile wieder von ganz links anfängt)
            string itemsSeparatedByNewLine = string.Join("\n\r", logItems);
            double userDuration = manager.GetUserDuration(user);
            // "Letzte Zeile Gesamtdauer der Arbeitszeit des Autors in Stunden"
            itemsSeparatedByNewLine += $"\n\rTotal working time: {userDuration}";
            return itemsSeparatedByNewLine;
        }
    }

    public enum Command
    {
        GetList, // getlist
        GetDuration, // getduration <author>
        GetUserDiary // getuserdiary <author>
    }
}