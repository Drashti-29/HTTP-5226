using DigitalArtShowcase.Data;
using DigitalArtShowcase.Interface;
using DigitalArtShowcase.Models;
using Microsoft.EntityFrameworkCore;

namespace DigitalArtShowcase.Services
{
    /// <summary>
    /// Service class to manage exhibitions in the digital art showcase.
    /// Provides methods to create, delete, retrieve, list, and update exhibitions.
    /// Each exhibition can include multiple artworks and their associated artists.
    /// </summary>
    public class ExhibitionService : IExhibitionService
    {
        private readonly ApplicationDbContext _context;

        public ExhibitionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse> CreateExhibition(ExhibitionDto exhibitionDto)
        {
            ServiceResponse serviceResponse = new();

            // Validate and fetch Artworks
            var artworks = await _context.Artworks
                .Where(a => exhibitionDto.Artworks.Select(adto => adto.ArtworkId).Contains(a.ArtworkId))
                .ToListAsync();

            if (artworks.Count != exhibitionDto.Artworks.Count)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Some artworks provided do not exist. Please check ArtworkIds.");
                return serviceResponse;
            }

            // Create a new Exhibition
            var exhibition = new Exhibition
            {
                ExhibitionName = exhibitionDto.ExhibitionName,
                Location = exhibitionDto.Location,
                Date = exhibitionDto.Date,
                Artworks = artworks
            };

            _context.Exhibitions.Add(exhibition);
            await _context.SaveChangesAsync();

            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.CreatedId = exhibition.ExhibitionId;
            serviceResponse.Messages.Add("Exhibition created successfully.");

            return serviceResponse;
        }

        public async Task<ServiceResponse> DeleteExhibition(int id)
        {
            ServiceResponse response = new();

            var exhibition = await _context.Exhibitions.FindAsync(id);
            if (exhibition == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Exhibition not found.");
                return response;
            }

            _context.Exhibitions.Remove(exhibition);
            await _context.SaveChangesAsync();

            response.Status = ServiceResponse.ServiceStatus.Deleted;
            response.Messages.Add("Exhibition deleted successfully.");

            return response;
        }

        public async Task<ExhibitionDto> GetExhibition(int id)
        {
            var exhibition = await _context.Exhibitions
                .Include(e => e.Artworks)
                .ThenInclude(a => a.Artist)
                .FirstOrDefaultAsync(e => e.ExhibitionId == id);

            if (exhibition == null)
            {
                return null;
            }

            return new ExhibitionDto
            {
                ExhibitionId = exhibition.ExhibitionId,
                ExhibitionName = exhibition.ExhibitionName,
                Location = exhibition.Location,
                Date = exhibition.Date,
                Artworks = exhibition.Artworks.Select(a => new ArtworkDto
                {
                    ArtworkId = a.ArtworkId,
                    Title = a.Title,
                    CreationYear = a.CreationYear,
                    Description = a.Description,
                    Price = a.Price,
                    ArtistId = a.ArtistId,
                    ArtistName = $"{a.Artist.FirstName} {a.Artist.LastName}"
                }).ToList()
            };
        }

        public async Task<IEnumerable<ExhibitionDto>> ListExhibitions()
        {
            // Fetch exhibitions with their related artworks
            var exhibitions = await _context.Exhibitions
                .Include(e => e.Artworks) // Include multiple artworks for each exhibition
                .ThenInclude(a => a.Artist) // Include artist details if needed
                .ToListAsync();

            // Map the exhibitions to ExhibitionDto
            return exhibitions.Select(exhibition => new ExhibitionDto
            {
                ExhibitionId = exhibition.ExhibitionId,
                ExhibitionName = exhibition.ExhibitionName,
                Location = exhibition.Location,
                Date = exhibition.Date,
                // Get Artwork details for each exhibition
                Artworks = exhibition.Artworks.Select(artwork => new ArtworkDto
                {
                    ArtworkId = artwork.ArtworkId,
                    Title = artwork.Title,
                    CreationYear = artwork.CreationYear,
                    Description = artwork.Description,
                    Price = artwork.Price,
                    ArtistId = artwork.ArtistId,
                    ArtistName = artwork.Artist != null
                        ? $"{artwork.Artist.FirstName} {artwork.Artist.LastName}"
                        : "Unknown Artist"
                }).ToList() // Map the artworks to ArtworkDto and create a list of them
            }).ToList();
        }

        public async Task<ServiceResponse> UpdateExhibitionDetails(int id, ExhibitionDto exhibitionDto)
        {
            ServiceResponse serviceResponse = new();

            var existingExhibition = await _context.Exhibitions
                .Include(e => e.Artworks)
                .FirstOrDefaultAsync(e => e.ExhibitionId == id);

            if (existingExhibition == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("Exhibition not found.");
                return serviceResponse;
            }

            // Update fields if provided
            if (!string.IsNullOrWhiteSpace(exhibitionDto.ExhibitionName))
            {
                existingExhibition.ExhibitionName = exhibitionDto.ExhibitionName;
            }
            if (!string.IsNullOrWhiteSpace(exhibitionDto.Location))
            {
                existingExhibition.Location = exhibitionDto.Location;
            }
            if (exhibitionDto.Date != DateTime.MinValue)
            {
                existingExhibition.Date = exhibitionDto.Date;
            }

            // Update artworks if provided
            if (exhibitionDto.Artworks != null && exhibitionDto.Artworks.Any())
            {
                var artworks = await _context.Artworks
                    .Where(a => exhibitionDto.Artworks.Select(adto => adto.ArtworkId).Contains(a.ArtworkId))
                    .ToListAsync();

                if (artworks.Count != exhibitionDto.Artworks.Count)
                {
                    serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                    serviceResponse.Messages.Add("Some artworks provided do not exist. Please check ArtworkIds.");
                    return serviceResponse;
                }

                existingExhibition.Artworks = artworks;
            }

            await _context.SaveChangesAsync();

            serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            serviceResponse.Messages.Add("Exhibition updated successfully.");

            return serviceResponse;
        }
    }
}
