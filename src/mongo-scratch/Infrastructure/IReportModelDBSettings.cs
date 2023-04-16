namespace mongo_scratch.Infrastructure;

public interface IReportModelDBSettings : IMongoSettings
{
}

public class ReportModelDBSettings : IReportModelDBSettings
{
    public string ConnectionString { get; set; }

    public string DatabaseName { get; set; }
}