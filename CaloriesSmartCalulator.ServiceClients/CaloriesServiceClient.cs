using System;
using System.Threading.Tasks;

namespace CaloriesSmartCalulator.ServiceClients
{
    //This imitates call to 3rd party
    public class CaloriesServiceClient : ICaloriesServiceClient
    {
        public Task<int> GetCaloriesForProduct(string productName)
        {
            var random = new Random();

            if (productName.Equals("exception", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception();
            }

            return Task.FromResult(random.Next(0, 500));
        }
    }
}
