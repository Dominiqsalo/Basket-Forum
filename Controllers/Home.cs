using BasketForum.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BasketForum.Controllers
{
    public class Home : Controller
    {
        public IActionResult Index()
        {
            using (dbkoppling db = new dbkoppling())
            {
                var inlagg = db.Inlagg.ToList();

                var senastePerKategori = inlagg
                    .GroupBy(i => i.kategori)
                    .ToDictionary(
                        g => g.Key,
                        g => g.OrderByDescending(i => i.id).FirstOrDefault()
                    );

                var antalPerKategori = inlagg
                    .GroupBy(i => i.kategori)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Count()
                    );

                var model = new StartSidaViewModel
                {
                    SenasteInlagg = senastePerKategori,
                    InlaggPerKategori = antalPerKategori
                };

                return View(model);
            }
        }
        [Authorize]
        public IActionResult Profil()
        {

            string loggedInUsername = User.Identity?.Name;

            using (dbkoppling database = new dbkoppling())
            {
                var user = database.Users.FirstOrDefault(u => u.anvandarnamn == loggedInUsername);
                if (user == null)
                return RedirectToAction("Login");

            // Dynamisk statistik
            int antalInlagg = database.Inlagg.Count(i => i.titel == user.anvandarnamn);
            int antalKommentarer = database.Kommentarer.Count(k => k.UserId == user.id);

            var kommentarLista = database.Kommentarer
                .Where(k => k.UserId == user.id)
                .OrderByDescending(k => k.Skapad)
                .ToList();

                var model = new UserProfileViewModel
            {
                User = user,
                AntalInlagg = antalInlagg,
                AntalKommentarer = antalKommentarer,
                Kommentarer = kommentarLista
                
            };

                return View(model);
            }
        }


        public IActionResult Login()
        {


            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(HamtaData user, string returnUrl = "")
        {
            //Kontrollera användarnamn
            bool userOk = CheckUserFromDB(user);
            if (userOk == true)
            {
                // Allt stämmer, logga in användaren
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, user.anvandarnamn));
                await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
                // Skicka användaren vidare till returnUrl om den finns annars till startsidan
                if (returnUrl != "")
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Profil", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Inloggningen inte godkänd";
                ViewData["ReturnUrl"] = returnUrl;
                return View();
            }

        }
        private bool CheckUserFromDB(HamtaData userInfo)
        {
            int count = 0;
            using (dbkoppling database = new dbkoppling())
            {
                var validUsers = database.Users.Where(u => u.anvandarnamn == userInfo.anvandarnamn).
                Where(u => u.losenord == userInfo.losenord);
                count = validUsers.Count();
            }
            if (count == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public IActionResult Registrera()
        {
            return View();
        }
        [HttpPost]
        public IActionResult skapauser(Users NyUser)
        {
            using (dbkoppling database = new dbkoppling())
            {
                database.Users.Add(NyUser);
                database.SaveChanges();
            }
            return RedirectToAction("Index");
          
        }
        public IActionResult subforum()
        {
            using (dbkoppling database = new dbkoppling())
            {
                var inlagg = database.Inlagg.ToList();
                return View(inlagg);
            }
           
        }
        public IActionResult visakommentar(int inlaggId)
        {
            using (var db = new dbkoppling())
            {
                var kommentarer = db.Kommentarer
                    .Where(k => k.InlaggId == inlaggId)
                    .OrderByDescending(k => k.Skapad)
                    .ToList();

                return View(kommentarer);
            }
        }

        public IActionResult ovrigt()
        {
            using (dbkoppling db = new dbkoppling())
            {
                var matchInlagg = db.Inlagg.Where(i => i.kategori == "Övrigt").ToList();
                return View(matchInlagg);
            }
        }
        public IActionResult statistik()
        {
            using (dbkoppling db = new dbkoppling())
            {
                var matchInlagg = db.Inlagg.Where(i => i.kategori == "Statistik").ToList();
                return View(matchInlagg);
            }
        }
        public IActionResult highlights()
        {
            using (dbkoppling db = new dbkoppling())
            {
                var matchInlagg = db.Inlagg.Where(i => i.kategori == "Highlight").ToList();
                return View(matchInlagg);
            }
        }
        public IActionResult senastmatch()
        {
            using (dbkoppling db = new dbkoppling())
            {
                var matchInlagg = db.Inlagg.Where(i => i.kategori == "Match").ToList();
                return View(matchInlagg);
            }
        }
        public async Task<IActionResult> SignOutUser()
        {
            await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
