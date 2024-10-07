using DigitalArtShowcase.Data;
using DigitalArtShowcase.Interface;
using DigitalArtShowcase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitalArtShowcase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtworkAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly IArtworkService _artworkService;

        // dependency injection of service interfaces
        public ArtworkAPIController(IArtworkService ArtworkService)
        {
            _artworkService = ArtworkService;
        }

        // GET: api/ArtistAPI
        [HttpGet(template: "List")]
        public async Task<IEnumerable<ArtworkDto>> ListArtworks()
        {
            return await _artworkService.ListArtworks();

        }

        // GET: api/ArtistAPI/5
        [HttpGet("{id}")]
        public async Task<ArtworkDto> GetArtwork(int id)

        {
            return await _artworkService.GetArtwork(id);
        }

        [HttpPut(template: "Update/{id}")]
        public async Task<ServiceResponse> UpdateArtworkDetails(int id, ArtworkDto artworkDto)
        {
            return await _artworkService.UpdateArtworkDetails(id, artworkDto);
        }

        [HttpPost(template: "Add")]
        public async Task<ServiceResponse> AddArtwork(ArtworkDto artworkDto)
        {
            return await _artworkService.AddArtwork(artworkDto);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ServiceResponse> DeleteArtwork(int id)
        {
            return await _artworkService.DeleteArtwork(id);
        }
    }
}
