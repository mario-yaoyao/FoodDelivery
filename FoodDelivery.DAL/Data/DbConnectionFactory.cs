using System.Data;
using Microsoft.Data.SqlClient;
using Hangfire.DAL.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Hangfire.DAL.Data;

public class DbConnectionFactory(IConfiguration configuration) : IDbConnectionFactory
{
    private readonly string dbConnectionString = configuration.GetConnectionString("FoodDeliveryDatabase")!;

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(dbConnectionString);
    }
}