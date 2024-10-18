using DigitalArtShowcase.Data;
using DigitalArtShowcase.Interface;
using DigitalArtShowcase.Models;
using Microsoft.EntityFrameworkCore;

namespace DigitalArtShowcase.Services
{
    public class ArtistService : IArtistService
    {
        private readonly ApplicationDbContext _context;

        // dependency injection of database context
        public ArtistService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new artist and saves it to the database.
        /// </summary>
        /// <param name="artistDto">The artist data transfer object containing artist details.</param>
        /// <returns>A ServiceResponse indicating the result of the creation operation.</returns>
        public async Task<ServiceResponse> CreateArtist(ArtistDto artistDto)
        {
            ServiceResponse serviceResponse = new();
            Artist artist = new Artist()
            {
                FirstName = artistDto.FirstName,
                LastName = artistDto.LastName,
                ArtistBio = artistDto.ArtistBio,
                Email = artistDto.Email
            };
            _context.Artists.Add(artist);
            await _context.SaveChangesAsync();

            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.CreatedId = artist.ArtistId;
            return serviceResponse;
        }

        /// <summary>
        /// Retrieves an artist by their ID.
        /// </summary>
        /// <param name="id">The ID of the artist to retrieve.</param>
        /// <returns>An ArtistDto containing the artist's details, or null if not found.</returns>
        public async Task<ArtistDto> GetArtist(int id)
        {
                // Fetch the artist along with artworks and their related exhibitions
                var artist = await _context.Artists
                    .Include(a => a.Artworks)
                    .ThenInclude(artwork => artwork.Exhibitions) // Include exhibitions for each artwork
                    .FirstOrDefaultAsync(a => a.ArtistId == id);

                // No artist found
                if (artist == null)
                {
                    return null;
                }

                // Create an instance of ArtistDto and populate the artist details
                ArtistDto artistDto = new ArtistDto
                {
                    ArtistId = artist.ArtistId,
                    FirstName = artist.FirstName,
                    LastName = artist.LastName,
                    ArtistBio = artist.ArtistBio,
                    Email = artist.Email,
                    Artworks = artist.Artworks.Select(artwork => new ArtworkDto
                    {
                        ArtworkId = artwork.ArtworkId,
                        Title = artwork.Title,
                        Description = artwork.Description,
                        CreationYear = artwork.CreationYear,
                        Price = artwork.Price,
                        ArtistId = artwork.ArtistId,
                    }).ToList()
                };

                return artistDto;
            }

        /// <summary>
        /// Retrieves a list of all artists.
        /// </summary>
        /// <returns>A collection of ArtistDto objects.</returns>
        public async Task<IEnumerable<ArtistDto>> ListArtists()
        {
            List<Artist> artists = await _context.Artists.ToListAsync();

            // empty list of data transfer object ArtistDto
            List<ArtistDto> artistDtos = new List<ArtistDto>();
            foreach (Artist artist in artists)
            {
                // create new instance of ArtistDto, add to list
                artistDtos.Add(new ArtistDto()
                {
                    ArtistId = artist.ArtistId,
                    FirstName = artist.FirstName,
                    LastName = artist.LastName,
                    ArtistBio = artist.ArtistBio,
                    Email = artist.Email
                });
            }

            return artistDtos;
        }

        /// <summary>
        /// Updates the details of an existing artist.
        /// </summary>
        /// <param name="id">The ID of the artist to update.</param>
        /// <param name="artistDto">The updated artist data transfer object.</param>
        /// <returns>A ServiceResponse indicating the result of the update operation.</returns>
        public async Task<ServiceResponse> UpdateArtistDetails(int id, ArtistDto artistDto)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            // Find the artist with the associated artworks using eager loading (Include)
            var existingArtist = await _context.Artists.Include(a => a.Artworks).FirstOrDefaultAsync(a => a.ArtistId == id);

            // Check if the artist exists
            if (existingArtist == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("Artist not found.");
                return serviceResponse;
            }

            // Update artist's basic details
            existingArtist.FirstName = artistDto.FirstName ?? existingArtist.FirstName;
            existingArtist.LastName = artistDto.LastName ?? existingArtist.LastName;
            existingArtist.ArtistBio = artistDto.ArtistBio ?? existingArtist.ArtistBio;
            existingArtist.Email = artistDto.Email ?? existingArtist.Email;

            // Handle artwork updates based on ArtworkIds in artistDto
            if (artistDto.ArtworkIds != null && artistDto.ArtworkIds.Any())
            {
                // Find artworks in the database that match the provided ArtworkIds
                var artworksToAdd = await _context.Artworks
                    .Where(aw => artistDto.ArtworkIds.Contains(aw.ArtworkId))
                    .ToListAsync();

                // Clear existing artworks
                existingArtist.Artworks.Clear();

                // Add the new artworks to the artist
                foreach (var artwork in artworksToAdd)
                {
                    existingArtist.Artworks.Add(artwork);
                }
            }

            try
            {
                // Save the changes to the database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("An error occurred while updating the artist.");
                return serviceResponse;
            }

            serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            serviceResponse.Messages.Add($"Artist {existingArtist.FirstName} {existingArtist.LastName} updated successfully.");

            return serviceResponse;
        }

        /// <summary>
        /// Deletes an artist by their ID.
        /// </summary>
        /// <param name="id">The ID of the artist to delete.</param>
        /// <returns>A ServiceResponse indicating the result of the deletion operation.</returns>
        public async Task<ServiceResponse> DeleteArtist(int id)
        {
            ServiceResponse response = new();
            // Artist must exist in the first place
            var artist = await _context.Artists.FindAsync(id);
            if (artist == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Artist cannot be deleted because it does not exist.");
                return response;
            }
            _context.Artists.Remove(artist);
            await _context.SaveChangesAsync();
            response.Status = ServiceResponse.ServiceStatus.Deleted;

            return response;
        }

    }
}