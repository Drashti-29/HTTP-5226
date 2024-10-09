using DigitalArtShowcase.Data;
using DigitalArtShowcase.Interface;
using DigitalArtShowcase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Numerics;

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
                Email = artistDto.Email,
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
            var artist =  await _context.Artists.FindAsync(id);
            // no artist found
            if (artist == null)
            {
                return null;
            }
            // create an instance of ArtistDto
            ArtistDto artistDto = new ArtistDto();
            artistDto.ArtistId = artist.ArtistId;
            artistDto.FirstName = artist.FirstName;
            artistDto.LastName = artist.LastName;
            artistDto.ArtistBio = artist.ArtistBio;
            artistDto.Email = artist.Email;
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
            Artist artist = new Artist()
            {
                ArtistId = artistDto.ArtistId,
                FirstName = artistDto.FirstName,
                LastName = artistDto.LastName,
                ArtistBio = artistDto.ArtistBio,
                Email = artistDto.Email,
            };
            // flags that the object has changed
            _context.Entry(artist).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;

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
