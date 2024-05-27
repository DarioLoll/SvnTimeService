using System;

namespace SvnTimeService.Server.Core
{
    public class SvnLogItem
    {
        public string Revision { get; }

        public string Author { get; }

        public DateTime Date { get; }

        public string CommitMessage { get; }

        public string UserComment { get; }

        public double Duration { get; }
        
        public SvnLogItem(string formattedString)
        {
            /* Beispiel
             * Legende: Revision;Author;Date;CommitMessage
                1311;normalverbrauchero;01.01.2017;#comment:Entwurf OberservationItem #duration: 2.7
             */
            try
            {
                string[] stringSplitAtSemicolon = formattedString.Split(';');
                Revision = stringSplitAtSemicolon[0];
                Author = stringSplitAtSemicolon[1];
                Date = DateTime.Parse(stringSplitAtSemicolon[2]);
                CommitMessage = stringSplitAtSemicolon[3];
            
                string[] stringSplitAtHashtag = stringSplitAtSemicolon[3].Split('#');
                //Mit Substring wird "comment:" weggeschnitten
                UserComment = stringSplitAtHashtag[1].Substring("comment:".Length).Trim();
                string durationString = stringSplitAtHashtag[2].Substring("duration:".Length).Trim();
                durationString = durationString.Replace('.', ','); //Damit auch Kommazahlen gelesen werden können
                Duration = double.Parse(durationString);
            }
            catch (Exception)
            {
                throw new ArgumentException($"Invalid string {formattedString} for SvnLogItem");
            }
        }

        public override string ToString()
        {
            /* Beispiel
             * Legende: Revision;Author;Date;CommitMessage
                1311;normalverbrauchero;01.01.2017;#comment:Entwurf OberservationItem #duration: 4
             */
            return $"{Revision};{Author};{Date.ToShortDateString()};#comment:{UserComment} #duration:{Duration}";
        }
    }
}