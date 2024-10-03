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

        // dependency injection of service interfaces
        public ArtistsAPIController(IArtistService ArtistService)
        {
            _artistService = ArtistService;
        }

        // GET: api/ArtistAPI
        [HttpGet(template: "List")]
        public async Task<IEnumerable<ArtistDto>> ListArtists()
        {
            return await _artistService.ListArtists();

        }

        // GET: api/ArtistAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ArtistDto>> GetArtist(int id)

        {
            return await _artistService.GetArtist(id);
        }
    }
}
