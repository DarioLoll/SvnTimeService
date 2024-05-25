namespace SvnTimeService.Server.Contracts
{
    public interface IServiceLogger
    {
        void LogSystemInfo(string log);

        void LogRequestInfo(string log);
    }
}