using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
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

        public GarageController(HttpClient httpClient, GarageService garageService)
        {
            _httpClient = httpClient;
            _garageService = garageService;
        }

        /// fetch garages from external government API
        [HttpGet("fetchFromAPI")]
        public async Task<IActionResult> FetchGaragesFromAPI()
        {
            Console.WriteLine("Fetching garages from government API...");
            try
            {
                var url = "https://data.gov.il/api/3/action/datastore_search?resource_id=bb68386a-a331-4bbc-b668-bba2766d517d&limit=5";
                var response = await _httpClient.GetStringAsync(url);

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

                Console.WriteLine($"Fetched {garages.Count} garages from government API");
                return Ok(garages);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP request error: {ex.Message}");
                return StatusCode(503, $"External API error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Server error while fetching garages: {ex.Message}");
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        /// get all garages from database
        [HttpGet("saved")]
        public async Task<IActionResult> GetSavedGarages()
        {
            Console.WriteLine("Fetching saved garages from database...");
            try
            {
                var garages = await _garageService.GetAllGaragesAsync();
                Console.WriteLine($"Retrieved {garages.Count} garages from database");
                return Ok(garages);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Server error while retrieving garages: {ex.Message}");
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        /// add a new garage to database
        [HttpPost("add")]
        public async Task<IActionResult> AddSelectedGarages([FromBody] List<Garage> selectedGarages)
        {
            Console.WriteLine($"Received {selectedGarages?.Count ?? 0} garages to add");

            try
            {
                if (selectedGarages == null || selectedGarages.Count == 0)
                {
                    Console.WriteLine("No garages provided in request");
                    return BadRequest("No garages provided");
                }

                var existingGarages = await _garageService.GetAllGaragesAsync();
                Console.WriteLine($"Currently {existingGarages.Count} garages in database");

                var garagesToAdd = selectedGarages
                    .Where(g => !existingGarages.Any(e => e.MisparMosah == g.MisparMosah))
                    .ToList();

                Console.WriteLine($"{garagesToAdd.Count} new garages will be added");

                if (garagesToAdd.Count == 0)
                    return Ok(new { message = "All selected garages already exist", addedCount = 0 });

                await _garageService.SaveGaragesAsync(garagesToAdd);
                Console.WriteLine("Garages successfully added to database");
                return Ok(new { message = $"{garagesToAdd.Count} garages added successfully", addedCount = garagesToAdd.Count });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Server error while adding garages: {ex.Message}");
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }
    }
}
