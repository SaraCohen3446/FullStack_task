using MongoDB.Driver;
using CarWorkshopAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CarWorkshopAPI.Services
{
    // service to manage garages in mongodb
    public class GarageService
    {
        private readonly IMongoCollection<Garage> _garages;

        //initialize mongo client and garages collection
        public GarageService(IConfiguration config)
        {
            var client = new MongoClient(config["MongoDB:ConnectionString"]);
            var database = client.GetDatabase(config["MongoDB:DatabaseName"]);
            _garages = database.GetCollection<Garage>("Garages"); // G גדולה
        }

        // save garages to mongodb
        public async Task SaveGaragesAsync(List<Garage> garages)
        {
            if (garages.Count > 0)
                await _garages.InsertManyAsync(garages);
        }

        // get all garages from mongodb
        public async Task<List<Garage>> GetAllGaragesAsync()
        {
            return await _garages.Find(_ => true).ToListAsync();
        }
    }
}
