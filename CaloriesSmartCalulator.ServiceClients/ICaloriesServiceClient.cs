using System.Threading.Tasks;

namespace CaloriesSmartCalulator.ServiceClients
{
    public interface ICaloriesServiceClient
    {
        Task<int> GetCaloriesForProduct(string productName);
    }
}
