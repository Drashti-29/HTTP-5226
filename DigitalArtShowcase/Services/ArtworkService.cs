using DigitalArtShowcase.Data;
using DigitalArtShowcase.Interface;
using DigitalArtShowcase.Models;
using Microsoft.EntityFrameworkCore;

namespace DigitalArtShowcase.Services
{
    public class ArtworkService : IArtworkService
    {
        private readonly ApplicationDbContext _context;

        // dependency injection of database context
        public ArtworkService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse> AddArtwork(ArtworkDto artworkDto)
        {
            ServiceResponse serviceResponse = new();
            // Check if the artist exists
            var artist = await _context.Artists.FindAsync(artworkDto.ArtistId);

            if (artist == null)
            {
                // Return a failure response if the artist doesn't exist
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Artist not found. Please provide a valid ArtistId.");
                return serviceResponse;
            }

            // Create a new Artwork entity
            Artwork artwork = new Artwork()
            {
                Title = artworkDto.Title,
                CreationYear = artworkDto.CreationYear,
                Description = artworkDto.Description,
                Price = artworkDto.Price,
                ArtistId = artworkDto.ArtistId
            };

            // Add the artwork to the database
            _context.Artworks.Add(artwork);
            await _context.SaveChangesAsync();

            // Set success response
            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.CreatedId = artwork.ArtworkId;

            // Add artist name to the response
            serviceResponse.Messages.Add($"Artwork created successfully for artist: {artist.FirstName} {artist.LastName}");

            return serviceResponse;

        }

        public async Task<ServiceResponse> DeleteArtwork(int id)
        {
            ServiceResponse response = new();
            // Artist must exist in the first place
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
            var artwork = await _context.Artworks
         .Include(a => a.Artist) // Include the Artist entity
         .FirstOrDefaultAsync(a => a.ArtworkId == id); // Use FirstOrDefaultAsync to match the artwork

            // No artwork found
            if (artwork == null)
            {
                return null;
            }

            // Create an instance of ArtworkDto
            ArtworkDto artworkDto = new ArtworkDto()
            {
                ArtworkId = artwork.ArtworkId,
                Title = artwork.Title,
                CreationYear = artwork.CreationYear,
                Description = artwork.Description,
                Price = artwork.Price,
                ArtistId = artwork.ArtistId,
                ArtistName = artwork.Artist != null ? artwork.Artist.FirstName + " " + artwork.Artist.LastName : "Unknown Artist" // Artist name
            };

            return artworkDto;
        }

        public async Task<IEnumerable<ArtworkDto>> ListArtworks()
        {
            List<Artwork> artworks = await _context.Artworks
         .Include(a => a.Artist) // Eagerly load the Artist entity
         .ToListAsync();

            // Create a list of ArtworkDto
            List<ArtworkDto> artworkDtos = new List<ArtworkDto>();

            foreach (Artwork artwork in artworks)
            {
                // Check if Artist is not null before accessing properties
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

            // Retrieve the existing artwork from the database
            var existingArtwork = await _context.Artworks.Include(a => a.Artist).FirstOrDefaultAsync(a => a.ArtworkId == id);

            if (existingArtwork == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("Artwork not found.");
                return serviceResponse;
            }

            // If an ArtistId is provided, update the artist
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

            // Update only provided fields (leave others unchanged)
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

            // Mark the entity as modified and save changes
            _context.Entry(existingArtwork).State = EntityState.Modif3.ied;
            await _context.SaveChangesAsync();

            // Include artist name in the service response
            serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            serviceResponse.Messages.Add($"Artwork updated successfully. Artist: {existingArtwork.Artist.FirstName} {existingArtwork.Artist.LastName}");

            return serviceResponse;
        }
    }
}
