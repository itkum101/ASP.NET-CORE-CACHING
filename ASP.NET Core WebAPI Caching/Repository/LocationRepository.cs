using ASP.NET_Core_WebAPI_Caching.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ASP.NET_Core_WebAPI_Caching.Repository
{
    public class LocationRepository
    {

        private readonly ApplicationDBContext _context;

        private readonly IMemoryCache _cache; 

        // Cache expiration time 

        private readonly TimeSpan _cacheExpiration = TimeSpan.FromSeconds(30);


        // Constructor to initialize ApplicationDBContext and IMEMORY CACHE

        public LocationRepository(ApplicationDBContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;

        }

        // Reterives the list of countries with caching 


        public async Task<List<Country>> GetCountriesAsync()
        {


            var cacheKey = "Countries"; 


            // Check if cacheKey is already set

            if(!_cache.TryGetValue(cacheKey, out List<Country> countries)) {

                // if not cache fetch from the main memory 

                countries = await _context.Countries.AsNoTracking().ToListAsync(); 

                _cache.Set(cacheKey, countries, _cacheExpiration);
                



                    }

            return countries ?? new List<Country>(); 


    }

        // Retrieves the list of states for a specific country, with caching.
        public async Task<List<State>> GetStatesAsync(int countryId)
        {
            // Defines a unique cache key based on the country ID.
            string cacheKey = $"States_{countryId}";

            // Checks if the states data for the given country ID is cached.
            if (!_cache.TryGetValue(cacheKey, out List<State>? states))
            {
                // Fetches states from the database if not cached
                // AsNoTracking(): Improves performance for read-only queries by disabling change tracking.
                states = await _context.States.Where(s => s.CountryId == countryId).AsNoTracking().ToListAsync();

                // Stores the fetched states in the cache with the expiration time.
                _cache.Set(cacheKey, states, _cacheExpiration);
            }

            // Returns the cached or fetched states, or an empty list if null.
            return states ?? new List<State>();
        }

        // Retrieves the list of cities for a specific state, with caching.
        public async Task<List<City>> GetCitiesAsync(int stateId)
        {
            // Defines a unique cache key based on the state ID.
            string cacheKey = $"Cities_{stateId}";

            // Checks if the cities data for the given state ID is cached.
            if (!_cache.TryGetValue(cacheKey, out List<City>? cities))
            {
                // Fetches cities from the database if not cached.
                // AsNoTracking(): Improves performance for read-only queries by disabling change tracking.
                cities = await _context.Cities.Where(c => c.StateId == stateId).AsNoTracking().ToListAsync();

                // Stores the fetched cities in the cache with the expiration time.
                _cache.Set(cacheKey, cities, _cacheExpiration);
            }

            // Returns the cached or fetched cities, or an empty list if null.
            return cities ?? new List<City>();
        }
    }

}
