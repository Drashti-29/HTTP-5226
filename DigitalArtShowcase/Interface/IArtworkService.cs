using DigitalArtShowcase.Models;

namespace DigitalArtShowcase.Interface
{
    public interface IArtworkService
    {
        Task<IEnumerable<ArtworkDto>> ListArtworks();
        Task<ArtworkDto> GetArtwork(int id);
        Task<ServiceResponse> UpdateArtworkDetails(int id, ArtworkDto artworkDto);
        Task<ServiceResponse> AddArtwork(ArtworkDto artworkDto);
        Task<ServiceResponse> DeleteArtwork(int id);
    }
}
