namespace SvnTimeService.Server.Contracts
{
    public interface IImportService
    {
        void Save(string[] lines);
        
        string[] Load();
    }
}