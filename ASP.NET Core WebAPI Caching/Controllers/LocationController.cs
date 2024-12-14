using ASP.NET_Core_WebAPI_Caching.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Core_WebAPI_Caching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly LocationRepository _repository;

        public LocationController(LocationRepository repository)
        {
            _repository = repository;
        }

        // Retrieves all countries.
        [HttpGet("countries")]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _repository.GetCountriesAsync();
            return Ok(countries);
        }

        // Retrieves states by country ID.
        [HttpGet("states/{countryId}")]
        public async Task<IActionResult> GetStates(int countryId)
        {
            var states = await _repository.GetStatesAsync(countryId);
            return Ok(states);
        }

        // Retrieves cities by state ID.
        [HttpGet("cities/{stateId}")]
        public async Task<IActionResult> GetCities(int stateId)
        {
            var cities = await _repository.GetCitiesAsync(stateId);
            return Ok(cities);
        }
    }

}
