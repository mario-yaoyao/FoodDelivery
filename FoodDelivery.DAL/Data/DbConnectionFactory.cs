using FoodDelivery.DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace FoodDelivery.DAL.Data;

public class DbConnectionFactory(IConfiguration configuration) : IDbConnectionFactory
{
    private readonly string dbProvider = configuration["DatabaseProvider"]!;
    private readonly string connectionString = configuration.GetConnectionString("FoodDeliveryDatabase")!;

    public IDbConnection CreateConnection()
    {
        return dbProvider switch
        {
            "SqlServer" => new SqlConnection(connectionString),
            "PostgreSql" => new NpgsqlConnection(connectionString),
            _ => throw new NotSupportedException($"Database provider '{dbProvider}' is not supported.")
        };
    }
}