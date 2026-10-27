using System.Data;
using FoodDelivery.DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace FoodDelivery.DAL.Data;

public class DbConnectionFactory(IConfiguration configuration) : IDbConnectionFactory
{
    private readonly string dbConnectionString = configuration.GetConnectionString("FoodDeliveryDatabase")!;

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(dbConnectionString);
    }
}