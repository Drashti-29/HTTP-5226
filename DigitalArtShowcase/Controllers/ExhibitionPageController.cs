using DigitalArtShowcase.Data;
using DigitalArtShowcase.Interface;
using DigitalArtShowcase.Models;
using DigitalArtShowcase.Services;
using Microsoft.AspNetCore.Mvc;

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

        public ExhibitionPageController(IExhibitionService ExhibitionService)
        {
            _exhibitionService = ExhibitionService;
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
        public ActionResult New()
        {
            var artwork = _context.Artworks.Select(a => new { ArtworkId = a.ArtworkId, Title = a.Title }).ToList();

            if (artwork == null || !artwork.Any())
            {
                // Handle the case when no artists are found
                ViewBag.ErrorMessage = "No artworks found in the database.";
            }

            ViewBag.Artworks = artwork;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(ExhibitionDto exhibitionDto)
        {
            ServiceResponse response = await _exhibitionService.CreateExhibition(exhibitionDto);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("List", "ExhibitionPage");
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }

        /// <summary>
        /// Updates the details of an existing exhibition.
        /// </summary>
        /// <param name="id">The ID of the exhibition to update.</param>
        /// <param name="exhibitionDto">The updated exhibition details.</param>
        /// <returns>A ServiceResponse indicating the result of the update operation.</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateDetails(int id, ExhibitionDto exhibitionDto)
        {
            return View(await _exhibitionService.UpdateExhibitionDetails(id, exhibitionDto));
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
