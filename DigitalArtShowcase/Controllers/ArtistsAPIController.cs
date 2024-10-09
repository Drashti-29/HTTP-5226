using DigitalArtShowcase.Data;
using DigitalArtShowcase.Interface;
using DigitalArtShowcase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace DigitalArtShowcase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly IArtistService _artistService;

        /// <summary>
        /// ArtistsAPIController handles artist-related API operations.
        /// This controller provides endpoints to:
        /// - List all artists
        /// - Get details of a specific artist by ID
        /// - Add a new artist
        /// - Update details of an existing artist
        /// - Delete an artist by ID
        /// dependency injection of service interfaces
        /// </summary>
        /// <param name="ArtistService">The artist service interface for performing operations.</param>

        public ArtistsAPIController(IArtistService ArtistService)
        {
            _artistService = ArtistService;
        }

        // GET: api/ArtistAPI
        /// <summary>
        /// Retrieves a list of all artists.
        /// </summary>
        /// <returns>A collection of ArtistDto objects.</returns>
        [HttpGet(template: "List")]
        public async Task<IEnumerable<ArtistDto>> ListArtists()
        {
            return await _artistService.ListArtists();

        }

        // GET: api/ArtistAPI/5
        /// <summary>
        /// Retrieves details of a specific artist by their ID.
        /// </summary>
        /// <param name="id">The ID of the artist to retrieve.</param>
        /// <returns>An ArtistDto object containing the artist's details.</returns>
        [HttpGet("{id}")]
        public async Task<ArtistDto> GetArtist(int id)

        {
            return await _artistService.GetArtist(id);
        }

        /// <summary>
        /// Adds a new artist.
        /// </summary>
        /// <param name="artistDto">The artist details to create.</param>
        /// <returns>A ServiceResponse indicating the result of the operation.</returns>
        [HttpPost(template: "Add")]
        public async Task<ServiceResponse> CreateArtist(ArtistDto artistDto)
        {
            return await _artistService.CreateArtist(artistDto);
        }

        /// <summary>
        /// Updates the details of an existing artist.
        /// </summary>
        /// <param name="id">The ID of the artist to update.</param>
        /// <param name="artistDto">The updated artist details.</param>
        /// <returns>A ServiceResponse indicating the result of the update operation.</returns>
        [HttpPut(template: "Update/{id}")]
        public async Task<ServiceResponse> UpdateArtistDetails(int id, ArtistDto artistDto)
        {
            return await _artistService.UpdateArtistDetails(id, artistDto);
        }

        /// <summary>
        /// Deletes an artist by their ID.
        /// </summary>
        /// <param name="id">The ID of the artist to delete.</param>
        /// <returns>A ServiceResponse indicating the result of the deletion.</returns>
        [HttpDelete("Delete/{id}")]
        public async Task<ServiceResponse> DeleteArtist(int id)
        {
            return await _artistService.DeleteArtist(id);
        }
    }
}
