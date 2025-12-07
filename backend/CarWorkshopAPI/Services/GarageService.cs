using MongoDB.Driver;
using CarWorkshopAPI.Models;

namespace CarWorkshopAPI.Services
{
    public class GarageService
    {
        private readonly IMongoCollection<Garage> _garages;

        public GarageService(IConfiguration config)
        {
            var client = new MongoClient(config["MongoDB:ConnectionString"]);
            var database = client.GetDatabase(config["MongoDB:DatabaseName"]);
            _garages = database.GetCollection<Garage>("Garages");
        }

        // Save list of garages to MongoDB
        public async Task SaveGaragesAsync(List<Garage> garages)
        {
            if (garages.Count > 0)
            {
                await _garages.InsertManyAsync(garages);
            }
        }

        // Retrieve all garages
        public async Task<List<Garage>> GetAllGaragesAsync()
        {
            return await _garages.Find(_ => true).ToListAsync();
        }
    }
}
