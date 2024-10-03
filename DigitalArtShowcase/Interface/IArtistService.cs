using DigitalArtShowcase.Data.Migrations;
using DigitalArtShowcase.Models;
using Microsoft.AspNetCore.Mvc;

namespace DigitalArtShowcase.Interface
{
    public interface IArtistService
    {
        // base CRUD
        Task<IEnumerable<ArtistDto>> ListArtists();
        Task<ActionResult<ArtistDto>> GetArtist(int id);

    }
}
