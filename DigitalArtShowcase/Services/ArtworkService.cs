using DigitalArtShowcase.Data;
using DigitalArtShowcase.Interface;
using DigitalArtShowcase.Models;
using Microsoft.EntityFrameworkCore;

namespace DigitalArtShowcase.Services
{
    /// <summary>
    /// Service class for managing artwork operations, including creation, retrieval, updating, and deletion of artworks.
    /// </summary>
    public class ArtworkService : IArtworkService
    {
        private readonly ApplicationDbContext _context;

        public ArtworkService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse> CreateArtwork(ArtworkDto artworkDto)
        {
            ServiceResponse serviceResponse = new();

            var artist = await _context.Artists.FindAsync(artworkDto.ArtistId);
            if (artist == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Artist not found. Please provide a valid ArtistId.");
                return serviceResponse;
            }

            Artwork artwork = new Artwork()
            {
                Title = artworkDto.Title,
                CreationYear = artworkDto.CreationYear,
                Description = artworkDto.Description,
                Price = artworkDto.Price,
                ArtistId = artworkDto.ArtistId
            };

            _context.Artworks.Add(artwork);
            await _context.SaveChangesAsync();

            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.CreatedId = artwork.ArtworkId;
            serviceResponse.Messages.Add($"Artwork created successfully for artist: {artist.FirstName} {artist.LastName}");

            return serviceResponse;
        }

        public async Task<ServiceResponse> DeleteArtwork(int id)
        {
            ServiceResponse response = new();

            var artwork = await _context.Artworks.FindAsync(id);
            if (artwork == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Artwork cannot be deleted because it does not exist.");
                return response;
            }
            _context.Artworks.Remove(artwork);
            await _context.SaveChangesAsync();
            response.Status = ServiceResponse.ServiceStatus.Deleted;

            return response;
        }

        public async Task<ArtworkDto> GetArtwork(int id)
        {
            // Fetch the artwork with the associated artist and exhibitions using eager loading
            var artwork = await _context.Artworks
                .Include(a => a.Artist)
                .Include(a => a.Exhibitions)
                .FirstOrDefaultAsync(a => a.ArtworkId == id);

            if (artwork == null)
            {
                return null; // or you can throw an exception if preferred
            }

            // Map the artwork entity to the ArtworkDto
            ArtworkDto artworkDto = new ArtworkDto()
            {
                ArtworkId = artwork.ArtworkId,
                Title = artwork.Title,
                CreationYear = artwork.CreationYear,
                Description = artwork.Description,
                Price = artwork.Price,
                ArtistId = artwork.ArtistId,
                ArtistName = artwork.Artist != null
                    ? $"{artwork.Artist.FirstName} {artwork.Artist.LastName}"
                    : "Unknown Artist",

                // Ensure Exhibitions is initialized to prevent null reference exception
                Exhibitions = artwork.Exhibitions != null
                    ? artwork.Exhibitions.Select(exhibition => new ExhibitionDto
                    {
                        ExhibitionId = exhibition.ExhibitionId,
                        ExhibitionName = exhibition.ExhibitionName,
                        Location = exhibition.Location,
                        Date = exhibition.Date
                    }).ToList()
                    : new List<ExhibitionDto>() // or simply return null or handle as needed
            };

            return artworkDto;
        }


        public async Task<IEnumerable<ArtworkDto>> ListArtworks()
        {
            List<Artwork> artworks = await _context.Artworks
                .Include(a => a.Artist)
                .ToListAsync();

            List<ArtworkDto> artworkDtos = new List<ArtworkDto>();

            foreach (Artwork artwork in artworks)
            {
                var artistName = artwork.Artist != null ? artwork.Artist.FirstName : "Unknown Artist";

                artworkDtos.Add(new ArtworkDto()
                {
                    ArtworkId = artwork.ArtworkId,
                    Title = artwork.Title,
                    CreationYear = artwork.CreationYear,
                    Description = artwork.Description,
                    Price = artwork.Price,
                    ArtistId = artwork.ArtistId,
                    ArtistName = artistName
                });
            }

            return artworkDtos;
        }

        public async Task<ServiceResponse> UpdateArtworkDetails(int id, ArtworkDto artworkDto)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            var existingArtwork = await _context.Artworks.Include(a => a.Artist).FirstOrDefaultAsync(a => a.ArtworkId == id);

            if (existingArtwork == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("Artwork not found.");
                return serviceResponse;
            }

            if (artworkDto.ArtistId != 0 && artworkDto.ArtistId != existingArtwork.ArtistId)
            {
                var artist = await _context.Artists.FindAsync(artworkDto.ArtistId);
                if (artist == null)
                {
                    serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                    serviceResponse.Messages.Add("Artist not found.");
                    return serviceResponse;
                }
                existingArtwork.ArtistId = artworkDto.ArtistId;
                existingArtwork.Artist = artist;
            }

            if (!string.IsNullOrWhiteSpace(artworkDto.Title))
            {
                existingArtwork.Title = artworkDto.Title;
            }
            if (!string.IsNullOrWhiteSpace(artworkDto.Description))
            {
                existingArtwork.Description = artworkDto.Description;
            }
            if (artworkDto.CreationYear != 0)
            {
                existingArtwork.CreationYear = artworkDto.CreationYear;
            }
            if (artworkDto.Price != 0)
            {
                existingArtwork.Price = artworkDto.Price;
            }

            await _context.SaveChangesAsync();

            serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            serviceResponse.Messages.Add($"Artwork updated successfully. Artist: {existingArtwork.Artist.FirstName} {existingArtwork.Artist.LastName}");

            return serviceResponse;
        }
    }
}
