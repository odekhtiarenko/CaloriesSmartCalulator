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
                Task.Delay(random.Next(1000, 5000));
                throw new Exception();
            }

            Task.Delay(random.Next(1000, 5000));

            return Task.FromResult(random.Next(0, 500));
        }
    }
}
