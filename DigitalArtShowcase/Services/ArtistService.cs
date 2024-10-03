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

        public async Task<ActionResult<ArtistDto>> GetArtist(int id)
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
    }
}
