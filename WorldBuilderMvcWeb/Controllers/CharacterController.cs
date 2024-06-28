using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WorldBuilderMvcWeb.Models;
using WorldBuilderMvcWeb.Models;
using WorldBuilderMvcWeb.Services;

namespace WorldBuilderMvcWeb.Controllers
{
    public class CharacterController : Controller
    {
        private readonly ApiService _apiService;

        public CharacterController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var characters = await _apiService.GetAllCharacters(page, pageSize);

            // Assuming you have a way to get the total count of characters
            int totalCharacters = await _apiService.GetTotalCharacterCount();
            int totalPages = (int)Math.Ceiling(totalCharacters / (double)pageSize);

            var viewModel = new CharacterListViewModel
            {
                Characters = characters,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(viewModel);
        }

        // Existing Create and Edit actions...

        public async Task<IActionResult> Details(int id)
        {
            var character = await _apiService.GetCharacter(id);
            if (character == null)
            {
                return NotFound();
            }
            return View(character);
        }

        //public async Task<IActionResult> Index(int id)
        //{
        //    var character = await _apiService.GetCharacter(id);
        //    if (character == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(character);
        //}

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Character character)
        {
            if (ModelState.IsValid)
            {
                var createdCharacter = await _apiService.CreateCharacter(character);
                return RedirectToAction("Index");
            }
            return View(character);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var character = await _apiService.GetCharacter(id);
            if (character == null)
            {
                return NotFound();
            }
            return View(character);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Character character)
        {
            if (ModelState.IsValid)
            {
                await _apiService.UpdateCharacter(id, character);
                return RedirectToAction("Index", new { id = id});
            }
            return View(character);
        }
    }
}
