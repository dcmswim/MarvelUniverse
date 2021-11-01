using MarvelUniverse.MarvelAPI;
using MarvelUniverse.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MarvelUniverse.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var marvel_obj = new Main();
            CharacterDataWrapper CharacterModel = await marvel_obj.GetCharacters();
            for (var i = 0; i < CharacterModel.Data.Results.Count; i++)
            {
                MarvelAPI.Character character = CharacterModel.Data.Results[i];
                
                
                //Console.WriteLine(character.Name);
                //Console.WriteLine(character.Description);
            }

            return View("Index");
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
