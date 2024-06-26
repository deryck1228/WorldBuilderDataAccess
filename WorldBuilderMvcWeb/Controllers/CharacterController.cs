using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WorldBuilderWeb.Models;
using WorldBuilderWeb.Services;

namespace WorldBuilderWeb.Controllers
{
    public class CharacterController : Controller
    {
        private readonly ApiService _apiService;

        public CharacterController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index(int id)
        {
            var character = await _apiService.GetCharacter(id);
            if (character == null)
            {
                return NotFound();
            }
            return View(character);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Character character)
        {
            if (ModelState.IsValid)
            {
                await _apiService.CreateCharacter(character);
                return RedirectToAction("Index", new { id = character.CharacterId });
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
                return RedirectToAction("Index", new { id = character.CharacterId });
            }
            return View(character);
        }
    }
}
