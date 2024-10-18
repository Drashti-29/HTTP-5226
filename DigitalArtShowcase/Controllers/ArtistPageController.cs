using DigitalArtShowcase.Data;
using DigitalArtShowcase.Interface;
using DigitalArtShowcase.Models;
using DigitalArtShowcase.Models.ViewModels;
using DigitalArtShowcase.Services;
using Microsoft.AspNetCore.Mvc;

namespace DigitalArtShowcase.Controllers
{
    public class ArtistPageController : Controller
    {
        private readonly IArtistService _artistService;
        private readonly IArtworkService _artworkService;

        /// <summary>
        /// ArtistsAPIController handles artist-related API operations.
        /// This controller provides endpoints to:
        /// - List all artists
        /// - Get details of a specific artist by ID
        /// - Add a new artist
        /// - Update details of an existing artist
        /// - Delete an artist by ID
        /// dependency injection of service interfaces
        /// </summary>
        /// <param name="ArtistService">The artist service interface for performing operations.</param
        public ArtistPageController(IArtistService ArtistService, IArtworkService ArtworkService)
        {
            _artistService = ArtistService;
            _artworkService = ArtworkService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        // GET: api/ArtistAPI
        /// <summary>
        /// Retrieves a list of all artists.
        /// </summary>
        /// <returns>A collection of ArtistDto objects.</returns>
        public async Task<IActionResult> List()
        {
            return View(await _artistService.ListArtists());
        }

        // GET: api/ArtistAPI/5
        /// <summary>
        /// Retrieves details of a specific artist by their ID.
        /// </summary>
        /// <param name="id">The ID of the artist to retrieve.</param>
        /// <returns>An ArtistDto object containing the artist's details.</returns>
        [HttpGet]
        public async Task<IActionResult> Detail(int id)

        {
            return View(await _artistService.GetArtist(id));
        }

        /// <summary>
        /// Adds a new artist.
        /// </summary>
        /// <param name="artistDto">The artist details to create.</param>
        /// <returns>A ServiceResponse indicating the result of the operation.</returns>

        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(ArtistDto artistDto)
        {
            ServiceResponse response = await _artistService.CreateArtist(artistDto);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("List", "ArtistPage");
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }

        /// <summary>
        /// Updates the details of an existing artist.
        /// </summary>
        /// <param name="id">The ID of the artist to update.</param>
        /// <param name="artistDto">The updated artist details.</param>
        /// <returns>A ServiceResponse indicating the result of the update operation.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ArtistDto? artistDto = await _artistService.GetArtist(id);
            IEnumerable<ArtworkDto> artworkDto = await _artworkService.ListArtworks();
            if (artistDto == null)
            {
                return View("Error");
            }
            else
            {
                ArtistEdit artistInfo = new ArtistEdit()
                {
                    Artist = artistDto,
                    Artworks = artworkDto,
                    SelectedArtworkIds = artistDto.ArtworkIds
                };
                return View(artistInfo);
            }
        }

        //POST OrderItemPage/Update/{id}
        [HttpPost]
        public async Task<IActionResult> Update(int id, ArtistDto artistDto)
        {
            ServiceResponse response = await _artistService.UpdateArtistDetails(id, artistDto);

            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                return RedirectToAction("List", "ArtistPage");
            }
            else
            {
                return View("something went wrong");
            }
        }

        /// <summary>
        /// Deletes an artist by their ID.
        /// </summary>
        /// <param name="id">The ID of the artist to delete.</param>
        /// <returns>A ServiceResponse indicating the result of the deletion.</returns>
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            // Fetch the Artist by ID
            ArtistDto? artist = await _artistService.GetArtist(id);

            if (artist == null)
            {
                return NotFound(); // Return 404 if not found
            }

            return View(artist); // Return the view with the artist details
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ServiceResponse response = await _artistService.DeleteArtist(id);

            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("List", "ArtistPage");
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }

        }

    }
}