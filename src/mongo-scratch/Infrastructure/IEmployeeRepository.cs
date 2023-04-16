using Microsoft.Extensions.Logging;
using mongo_scratch.Models;

namespace mongo_scratch.Infrastructure;

public interface IEmployeeRepository : IEntityRepository<Employee, EmployeeId>
{
}

public class EmployeeRepository : EntityMongoRepository<Employee, EmployeeId>, IEmployeeRepository
{
    public EmployeeRepository(IReportMongoContext mongoContext, ILoggerFactory loggerFactory) : base(mongoContext,
        loggerFactory)
    {
    }
}