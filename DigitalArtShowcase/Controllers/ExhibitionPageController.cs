using DigitalArtShowcase.Data;
using DigitalArtShowcase.Interface;
using DigitalArtShowcase.Models;
using DigitalArtShowcase.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DigitalArtShowcase.Controllers
{
    public class ExhibitionPageController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }
        private readonly ApplicationDbContext _context;

        private readonly IExhibitionService _exhibitionService;

        /// <summary>
        /// ExhibitionAPIController manages exhibition-related API operations.
        /// This controller provides endpoints to:
        /// - List all exhibitions
        /// - Get details of a specific exhibition by ID
        /// - Add a new exhibition
        /// - Update details of an existing exhibition
        /// - Delete an exhibition by ID
        /// It utilizes dependency injection to interact with the exhibition service layer.
        /// </summary>
        /// <param name="ExhibitionService">The exhibition service interface for performing operations.</param>

        public ExhibitionPageController(IExhibitionService ExhibitionService, ApplicationDbContext context)
        {
            _exhibitionService = ExhibitionService;
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of all exhibitions.
        /// </summary>
        /// <returns>A collection of ExhibitionDto objects.</returns>
        /// GET: api/ExhibitionAPI
        [HttpGet]
        public async Task<IActionResult> List()
        {
            return View(await _exhibitionService.ListExhibitions());

        }

        /// <summary>
        /// Retrieves details of a specific exhibition by its ID.
        /// </summary>
        /// <param name="id">The ID of the exhibition to retrieve.</param>
        /// <returns>An ExhibitionDto object containing the exhibition's details.</returns>
        [HttpGet]
        public async Task<IActionResult> Details(int id)

        {
            return View(await _exhibitionService.GetExhibition(id));
        }

        /// <summary>
        /// Adds a new exhibition.
        /// </summary>
        /// <param name="exhibitionDto">The exhibition details to create.</param>
        /// <returns>A ServiceResponse indicating the result of the operation.</returns>
        public IActionResult New()
        {
            // Fetch and map artworks to ArtworkDto
            var availableArtworks = _context.Artworks
                .Select(a => new ArtworkDto
                {
                    ArtworkId = a.ArtworkId,
                    Title = a.Title
                    // Map other properties as needed
                })
                .ToList();

            var model = new ExhibitionDto
            {
                Artworks = availableArtworks // Assign the mapped artworks
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExhibitionDto exhibitionDto)
        {

            if (!ModelState.IsValid)
            {
                // Repopulate available artworks in case of validation errors
                exhibitionDto.Artworks = await _context.Artworks
                    .Select(a => new ArtworkDto
                    {
                        ArtworkId = a.ArtworkId,
                        Title = a.Title
                    }).ToListAsync();

                return View(exhibitionDto);
            }

            // Validate and fetch Artworks
            var artworks = await _context.Artworks
                .Where(a => exhibitionDto.Artworks.Select(adto => adto.ArtworkId).Contains(a.ArtworkId))
                .ToListAsync();

            if (artworks.Count != exhibitionDto.Artworks.Count)
            {
                ModelState.AddModelError(string.Empty, "Some artworks provided do not exist. Please check ArtworkIds.");
                exhibitionDto.Artworks = await _context.Artworks
                    .Select(a => new ArtworkDto
                    {
                        ArtworkId = a.ArtworkId,
                        Title = a.Title
                    }).ToListAsync();

                return View(exhibitionDto);
            }

            // Create a new Exhibition
            var exhibition = new Exhibition
            {
                ExhibitionName = exhibitionDto.ExhibitionName,
                Location = exhibitionDto.Location,
                Date = exhibitionDto.Date,
                Artworks = artworks // Assign the fetched artworks to the exhibition
            };

            _context.Exhibitions.Add(exhibition);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Updates the details of an existing exhibition.
        /// </summary>
        /// <param name="id">The ID of the exhibition to update.</param>
        /// <param name="exhibitionDto">The updated exhibition details.</param>
        /// <returns>A ServiceResponse indicating the result of the update operation.</returns>

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // Fetch the existing exhibition along with its artworks
            var existingExhibition = await _context.Exhibitions
                .Include(e => e.Artworks)
                .FirstOrDefaultAsync(e => e.ExhibitionId == id);

            if (existingExhibition == null)
            {
                return NotFound();
            }

            // Map existing exhibition to DTO
            var exhibitionDto = new ExhibitionDto
            {
                ExhibitionId = existingExhibition.ExhibitionId,
                ExhibitionName = existingExhibition.ExhibitionName,
                Location = existingExhibition.Location,
                Date = existingExhibition.Date,
                Artworks = existingExhibition.Artworks.Select(a => new ArtworkDto
                {
                    ArtworkId = a.ArtworkId,
                    Title = a.Title // Assuming Artwork has a property called ArtworkName
                }).ToList()
            };

            return View(exhibitionDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ExhibitionDto exhibitionDto)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return RedirectToAction("List", "ExhibitionPage"); ;
            }

            // Call the service method to update the exhibition
            var serviceResponse = await _exhibitionService.UpdateExhibitionDetails(exhibitionDto.ExhibitionId, exhibitionDto);

            // Handle response status
            if (serviceResponse.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                ModelState.AddModelError(string.Empty, "Exhibition not found.");
                return RedirectToAction("List", "ExhibitionPage");
            }

            if (serviceResponse.Status == ServiceResponse.ServiceStatus.Error)
            {
                ModelState.AddModelError(string.Empty, string.Join(", ", serviceResponse.Messages));
                return RedirectToAction("List", "ExhibitionPage");
            }

            // Redirect to the index or another appropriate action
            return RedirectToAction("List", "ExhibitionPage");
        }

        /// <summary>
        /// Deletes an exhibition by its ID.
        /// </summary>
        /// <param name="id">The ID of the exhibition to delete.</param>
        /// <returns>A ServiceResponse indicating the result of the deletion.</returns>
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {   // Fetch the exhibition by ID
            ExhibitionDto? exhibition = await _exhibitionService.GetExhibition(id);

            if (exhibition == null)
            {
                return NotFound(); // Return 404 if not found
            }

            return View(exhibition); // Return the view with the exhibition details
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ServiceResponse response = await _exhibitionService.DeleteExhibition(id);

            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("List", "ExhibitionPage");
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }

        }

    }
}
