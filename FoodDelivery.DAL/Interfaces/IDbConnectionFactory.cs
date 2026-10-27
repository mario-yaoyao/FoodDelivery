using System.Data;

namespace FoodDelivery.DAL.Interfaces
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
