using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using CarWorkshopAPI.Models;
using CarWorkshopAPI.Services;

namespace CarWorkshopAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GarageController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly GarageService _garageService;

        // Constructor injecting HttpClient and GarageService
        public GarageController(HttpClient httpClient, GarageService garageService)
        {
            _httpClient = httpClient;
            _garageService = garageService;
        }

        // GET api/garage/fetch
        [HttpGet("fetch")]
        public async Task<IActionResult> FetchGarages()
        {
            var url = "https://data.gov.il/api/3/action/datastore_search?resource_id=bb68386a-a331-4bbc-b668-bba2766d517d&limit=5";
            var response = await _httpClient.GetStringAsync(url);

            // Parse JSON to list of Garage
            var jsonDoc = JsonDocument.Parse(response);
            var records = jsonDoc.RootElement.GetProperty("result").GetProperty("records");

            var garages = new List<Garage>();
            foreach (var record in records.EnumerateArray())
            {
                garages.Add(new Garage
                {
                    MisparMosah = record.GetProperty("mispar_mosah").GetInt32(),
                    ShemMosah = record.GetProperty("shem_mosah").GetString(),
                    CodSugMosah = record.GetProperty("cod_sug_mosah").GetInt32(),
                    SugMosah = record.GetProperty("sug_mosah").GetString(),
                    Ktovet = record.GetProperty("ktovet").GetString(),
                    Yishuv = record.GetProperty("yishuv").GetString(),
                    Telephone = record.GetProperty("telephone").GetString(),
                    Mikud = record.GetProperty("mikud").GetInt32(),
                    CodMiktzoa = record.GetProperty("cod_miktzoa").GetInt32(),
                    Miktzoa = record.GetProperty("miktzoa").GetString(),
                    MenahelMiktzoa = record.GetProperty("menahel_miktzoa").GetString(),
                    RashamHavarot = record.GetProperty("rasham_havarot").GetInt64(),
                    Testime = record.GetProperty("TESTIME").GetString()
                });
            }

            // Save to MongoDB
            if (garages.Count > 0)
            {
                await _garageService.SaveGaragesAsync(garages);
            }

            // Return saved garages
            return Ok(garages);
        }
    }
}
