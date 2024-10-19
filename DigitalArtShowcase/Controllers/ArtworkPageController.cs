using DigitalArtShowcase.Data;
using DigitalArtShowcase.Interface;
using DigitalArtShowcase.Models;
using DigitalArtShowcase.Models.ViewModels;
using DigitalArtShowcase.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DigitalArtShowcase.Controllers
{
    public class ArtworkPageController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }
        private readonly ApplicationDbContext _context;
        private readonly IArtworkService _artworkService;
        private readonly IArtistService _artistService;


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
        public ArtworkPageController(ApplicationDbContext context, IArtworkService ArtworkService, IArtistService ArtistService)
        {
            _artworkService = ArtworkService;
            _artistService = ArtistService;
            _context = _context;
        }

        // GET: api/ArtistAPI
        /// <summary>
        /// Retrieves a list of all artworks.
        /// </summary>
        /// <returns>A collection of ArtworkDto objects.</returns>
        [HttpGet]
        public async Task<IActionResult> List()
        {
            return View(await _artworkService.ListArtworks());

        }

        // GET: api/ArtistAPI/5
        /// <summary>
        /// Retrieves details of a specific artwork by its ID.
        /// </summary>
        /// <param name="id">The ID of the artwork to retrieve.</param>
        /// <returns>An ArtworkDto object containing the artwork's details.</returns>
        [HttpGet]
        public async Task<IActionResult> Details(int id)

        {
            return View(await _artworkService.GetArtwork(id));
        }

        /// <summary>
        /// Updates the details of an existing artwork.
        /// </summary>
        /// <param name="id">The ID of the artwork to update.</param>
        /// <param name="artworkDto">The updated artwork details.</param>
        /// <returns>A ServiceResponse indicating the result of the update operation.</returns>
        // GET: Edit Artwork
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var artwork = await _context.Artworks
        .Include(a => a.Artist)
        .Include(a => a.Exhibitions) // Include associated exhibitions if applicable
        .FirstOrDefaultAsync(a => a.ArtworkId == id);

            if (artwork == null)
            {
                return NotFound();
            }

            // Prepare the ArtworkDto
            var artworkDto = new ArtworkDto
            {
                ArtworkId = artwork.ArtworkId,
                Title = artwork.Title,
                Description = artwork.Description,
                CreationYear = artwork.CreationYear,
                Price = artwork.Price,
                ArtistId = artwork.ArtistId,
                ArtistName = artwork.Artist.FirstName + " " + artwork.Artist.LastName,
                Exhibitions = artwork.Exhibitions.Select(e => new ExhibitionDto
                {
                    ExhibitionName = e.ExhibitionName,
                    Date = e.Date
                }).ToList()
            };
            // Retrieve the list of artists for radio button selection
            var artistList = await _artistService.ListArtists();

            // Pass both the ArtworkDto and the ArtistList to the view
            ViewBag.ArtistList = artistList;

            return View(artworkDto);
        }

        // POST: Update Artwork
        [HttpPost]
        public async Task<IActionResult> Edit(int id, ArtworkDto artworkDto)
        {
            if (ModelState.IsValid)
            {
                var serviceResponse = await _artworkService.UpdateArtworkDetails(id,artworkDto);
                if (serviceResponse.Status == ServiceResponse.ServiceStatus.Updated)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", string.Join(", ", serviceResponse.Messages));
            }

            // Repopulate the artist list in case of a validation error
            artworkDto.ArtistName = (await _artistService.GetArtist(id)).FirstName;
            return View(artworkDto);
        }


        /// <summary>
        /// Adds a new artwork.
        /// </summary>
        /// <param name="artworkDto">The artwork details to create.</param>
        /// <returns>A ServiceResponse indicating the result of the operation.</returns>
        public async Task<ActionResult> New()
        {
            var artworkDto = new ArtworkDto
            {
                Exhibitions = new List<ExhibitionDto>() // Ensure the list is initialized
            };
            return View(artworkDto);

        }

        [HttpPost]
        public async Task<IActionResult> Add(ArtworkDto artworkDto)
        {
            ServiceResponse response = await _artworkService.CreateArtwork(artworkDto);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("List", "ArtworkPage");
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }

        /// <summary>
        /// Deletes an artwork by its ID.
        /// </summary>
        /// <param name="id">The ID of the artwork to delete.</param>
        /// <returns>A ServiceResponse indicating the result of the deletion.</returns>
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            // Fetch the Artwork by ID
            ArtworkDto? artwork = await _artworkService.GetArtwork(id);

            if (artwork == null)
            {
                return NotFound(); // Return 404 if not found
            }

            return View(artwork); // Return the view with the artwork details
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ServiceResponse response = await _artworkService.DeleteArtwork(id);

            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("List", "ArtworkPage");
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }

        }
    }
}
