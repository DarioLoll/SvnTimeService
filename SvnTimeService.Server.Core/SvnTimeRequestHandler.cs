using System;
using System.Collections.Generic;

namespace SvnTimeService.Server.Core
{
    public enum Command
    {
        GetList, // getlist
        GetDuration, // getduration <author>
        GetUserDiary // getuserdiary <author>
    }
    
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
                
                //Hier wird der erste Teil des Strings in ein Enum umgewandelt
                //(Command), wo alle Kommandos definiert sind
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
                        string author = splitString[1];
                        return GetDuration(author);
                    case Command.GetUserDiary:
                        string user = splitString[1];
                        return GetUserDiary(user);
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
            //Hier wird die Liste der LogItems aus dem SvnTimeManager geholt
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
            //Hier wird die Arbeitszeit des Autors und die durchschnittliche Arbeitszeit geholt
            SvnTimeManager manager = SvnTimeManager.GetInstance();
            double userDuration = manager.GetUserDuration(user);
            double averageDuration = manager.GetAverageDuration();
            return $"User: {userDuration} | Average: {averageDuration}";
        }
        
        private string GetUserDiary(string user)
        {
            //Hier werden die Einträge vom "user" geholt
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
}