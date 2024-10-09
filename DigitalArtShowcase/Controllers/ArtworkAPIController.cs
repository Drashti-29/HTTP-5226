﻿using DigitalArtShowcase.Data;
using DigitalArtShowcase.Interface;
using DigitalArtShowcase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitalArtShowcase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtworkAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly IArtworkService _artworkService;

        /// <summary>
        /// ArtworkAPIController manages artwork-related API operations.
        /// This controller provides endpoints to:
        /// - List all artworks
        /// - Get details of a specific artwork by ID
        /// - Add a new artwork
        /// - Update details of an existing artwork
        /// - Delete an artwork by ID
        /// It utilizes dependency injection to interact with the artwork service layer.
        /// </summary>
        /// <param name="ArtworkService">The artwork service interface for performing operations.</param>
        public ArtworkAPIController(IArtworkService ArtworkService)
        {
            _artworkService = ArtworkService;
        }

        // GET: api/ArtistAPI
        /// <summary>
        /// Retrieves a list of all artworks.
        /// </summary>
        /// <returns>A collection of ArtworkDto objects.</returns>
        [HttpGet(template: "List")]
        public async Task<IEnumerable<ArtworkDto>> ListArtworks()
        {
            return await _artworkService.ListArtworks();

        }

        // GET: api/ArtistAPI/5
        /// <summary>
        /// Retrieves details of a specific artwork by its ID.
        /// </summary>
        /// <param name="id">The ID of the artwork to retrieve.</param>
        /// <returns>An ArtworkDto object containing the artwork's details.</returns>
        [HttpGet("{id}")]
        public async Task<ArtworkDto> GetArtwork(int id)

        {
            return await _artworkService.GetArtwork(id);
        }

        /// <summary>
        /// Updates the details of an existing artwork.
        /// </summary>
        /// <param name="id">The ID of the artwork to update.</param>
        /// <param name="artworkDto">The updated artwork details.</param>
        /// <returns>A ServiceResponse indicating the result of the update operation.</returns>
        [HttpPut(template: "Update/{id}")]
        public async Task<ServiceResponse> UpdateArtworkDetails(int id, ArtworkDto artworkDto)
        {
            return await _artworkService.UpdateArtworkDetails(id, artworkDto);
        }

        /// <summary>
        /// Adds a new artwork.
        /// </summary>
        /// <param name="artworkDto">The artwork details to create.</param>
        /// <returns>A ServiceResponse indicating the result of the operation.</returns>
        [HttpPost(template: "Add")]
        public async Task<ServiceResponse> CreateArtwork(ArtworkDto artworkDto)
        {
            return await _artworkService.CreateArtwork(artworkDto);
        }

        /// <summary>
        /// Deletes an artwork by its ID.
        /// </summary>
        /// <param name="id">The ID of the artwork to delete.</param>
        /// <returns>A ServiceResponse indicating the result of the deletion.</returns>
        [HttpDelete("Delete/{id}")]
        public async Task<ServiceResponse> DeleteArtwork(int id)
        {
            return await _artworkService.DeleteArtwork(id);
        }
    }
}