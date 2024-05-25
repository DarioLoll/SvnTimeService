using System.IO;
using SvnTimeService.Server.Contracts;

namespace SvnTimeService.Server.Core
{
    public class FileImportService : IImportService
    {
        private string _path;

        public FileImportService(string path)
        {
            _path = path;
        }
        
        public void Save(string[] lines)
        {
            File.WriteAllLines(_path, lines);
        }

        public string[] Load()
        {
            return File.ReadAllLines(_path);
        }
    }
}