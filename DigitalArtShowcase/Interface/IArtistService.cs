using DigitalArtShowcase.Data.Migrations;
using DigitalArtShowcase.Models;
using Microsoft.AspNetCore.Mvc;
using Artist = DigitalArtShowcase.Models.Artist;

namespace DigitalArtShowcase.Interface
{
    public interface IArtistService
    {
        // base CRUD
        Task<IEnumerable<ArtistDto>> ListArtists();
        Task<ArtistDto> GetArtist(int id);
        Task<ServiceResponse> UpdateArtistDetails(int id, ArtistDto artistDto);
        Task<ServiceResponse> AddArtist(ArtistDto artistDto);
        Task<ServiceResponse> DeleteArtist(int id);
    }
}
