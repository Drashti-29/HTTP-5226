using DigitalArtShowcase.Data.Migrations;
using DigitalArtShowcase.Models;
using Microsoft.AspNetCore.Mvc;
using Artist = DigitalArtShowcase.Models.Artist;

namespace DigitalArtShowcase.Interface
{
    /// <summary>
    /// IArtistService defines the contract for artist-related operations.
    /// It includes methods for managing artists through basic CRUD functionalities.
    /// </summary>
    public interface IArtistService
    {
        /// <summary>
        /// Retrieves a list of all artists.
        /// </summary>
        /// <returns>A collection of ArtistDto objects.</returns>
        Task<IEnumerable<ArtistDto>> ListArtists();

        /// <summary>
        /// Retrieves details of a specific artist by ID.
        /// </summary>
        /// <param name="id">The ID of the artist to retrieve.</param>
        /// <returns>An ArtistDto object containing the artist's details.</returns>
        Task<ArtistDto> GetArtist(int id);

        /// <summary>
        /// Updates the details of an existing artist.
        /// </summary>
        /// <param name="id">The ID of the artist to update.</param>
        /// <param name="artistDto">The updated artist details.</param>
        /// <returns>A ServiceResponse indicating the result of the update operation.</returns>
        Task<ServiceResponse> UpdateArtistDetails(int id, ArtistDto artistDto);

        /// <summary>
        /// Adds a new artist.
        /// </summary>
        /// <param name="artistDto">The artist details to create.</param>
        /// <returns>A ServiceResponse indicating the result of the operation.</returns>
        Task<ServiceResponse> CreateArtist(ArtistDto artistDto);

        /// <summary>
        /// Deletes an artist by its ID.
        /// </summary>
        /// <param name="id">The ID of the artist to delete.</param>
        /// <returns>A ServiceResponse indicating the result of the deletion.</returns>
        Task<ServiceResponse> DeleteArtist(int id);
    }
}
