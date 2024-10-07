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

        public async Task<ServiceResponse> AddArtist(ArtistDto artistDto)
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

        public async Task<IEnumerable<ArtistDto>> ListArtists()
        {
            List<Artist> artists = await _context.Artists.ToListAsync();

            // empty list of data transfer object ArtistDto
            List<ArtistDto> artistDtos = new List<ArtistDto>();
            foreach (Artist artist in artists)
            {
                // create new instance of OrderItemDto, add to list
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
    }
}
