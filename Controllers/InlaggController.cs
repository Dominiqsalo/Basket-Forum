using BasketForum.Models;
using Microsoft.AspNetCore.Mvc;

namespace BasketForum.Controllers
{
    public class InlaggController : Controller
    {
        public IActionResult Inlagg()
        {
            return View();
        }
        public IActionResult skapainlagg()
        {
            return View(new Inlagg());
        }
        [HttpPost]
        public IActionResult skapapost(Inlagg NyPost)
        {
            Console.WriteLine("POST-försök: " + NyPost.titel);

            if (!ModelState.IsValid)
            {
                foreach (var kvp in ModelState)
                {
                    foreach (var error in kvp.Value.Errors)
                    {
                        Console.WriteLine($"VALIDERINGSFEL: {kvp.Key} => {error.ErrorMessage}");
                    }
                }

                return View("skapainlagg", NyPost);
            }

            NyPost.skapad = DateTime.Now;
            NyPost.skapare = User.Identity.Name ?? "Anonym";

            using (dbkoppling database = new dbkoppling())
            {
                database.Inlagg.Add(NyPost);
                database.SaveChanges();
            }

            return RedirectToAction("subforum", "Home");

        }



    }
}
