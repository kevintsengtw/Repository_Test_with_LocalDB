using System.Data;

namespace NorthwindRepository.Database
{
    public interface IDatabaseConnectionFactory
    {
        IDbConnection Create();
    }
}