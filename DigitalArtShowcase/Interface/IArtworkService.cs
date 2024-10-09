using DigitalArtShowcase.Models;

namespace DigitalArtShowcase.Interface
{

    /// <summary>
    /// IArtworkService defines the contract for artwork-related operations.
    /// It includes methods for managing artworks through basic CRUD functionalities.
    /// </summary>
    public interface IArtworkService
    {
        /// <summary>
        /// Retrieves a list of all artworks.
        /// </summary>
        /// <returns>A collection of ArtworkDto objects.</returns>
        Task<IEnumerable<ArtworkDto>> ListArtworks();

        /// <summary>
        /// Retrieves details of a specific artwork by ID.
        /// </summary>
        /// <param name="id">The ID of the artwork to retrieve.</param>
        /// <returns>An ArtworkDto object containing the artwork's details.</returns>
        Task<ArtworkDto> GetArtwork(int id);

        /// <summary>
        /// Updates the details of an existing artwork.
        /// </summary>
        /// <param name="id">The ID of the artwork to update.</param>
        /// <param name="artworkDto">The updated artwork details.</param>
        /// <returns>A ServiceResponse indicating the result of the update operation.</returns>
        Task<ServiceResponse> UpdateArtworkDetails(int id, ArtworkDto artworkDto);

        /// <summary>
        /// Adds a new artwork.
        /// </summary>
        /// <param name="artworkDto">The artwork details to create.</param>
        /// <returns>A ServiceResponse indicating the result of the operation.</returns>
        Task<ServiceResponse> CreateArtwork(ArtworkDto artworkDto);

        /// <summary>
        /// Deletes an artwork by its ID.
        /// </summary>
        /// <param name="id">The ID of the artwork to delete.</param>
        /// <returns>A ServiceResponse indicating the result of the deletion.</returns>
        Task<ServiceResponse> DeleteArtwork(int id);
    }
}

