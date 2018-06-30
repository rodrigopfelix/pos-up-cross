using CrossApp.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CrossApp.Infra.API
{
    [Headers("Content-Type: application/json")]
    public interface ITmdbApi
    {
        [Get("/tv/popular?api_key={apikey}")]
        Task<SerieResponse> GetSerieResponseAsync(string apiKey);
    }
}
