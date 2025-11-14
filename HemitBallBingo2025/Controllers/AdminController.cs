using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HemitBallBingo2025.Controllers
{
    public class AdminController : Controller
    {
        private readonly HemitBallbingoContext _context;
        private readonly string _adminPassword;

        public AdminController(HemitBallbingoContext context, IConfiguration config)
        {
            _context = context;
            _adminPassword = config["AdminPassword"];
        }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string password)
        {
            if (password == _adminPassword)
            {
                HttpContext.Session.SetString("Admin", "true");
                return RedirectToAction("History");
            }
            ViewBag.Error = "Feil passord";
            return View();
        }

        public IActionResult Index()
        {
            ViewBag.IsAdmin = HttpContext.Session.GetString("Admin") == "true";
            return View();
        }

        public async Task<IActionResult> History()
        {
            if (HttpContext.Session.GetString("Admin") != "true")
                return RedirectToAction("Login");

            var draws = await _context.LotteryDraws.Include(d => d.Tickets).ToListAsync();
            return View(draws);
        }

        public async Task<IActionResult> Draw(int id)
        {
            if (HttpContext.Session.GetString("Admin") != "true")
                return RedirectToAction("Login");

            var draw = await _context.LotteryDraws.Include(d => d.Tickets).FirstOrDefaultAsync(d => d.Id == id);
            if (draw == null) return NotFound();

            var shuffled = draw.Tickets.OrderBy(x => Guid.NewGuid()).ToList();
            var winners = shuffled.Take(3).ToList();
            var losers = shuffled.Skip(3).ToList();

            ViewBag.Losers = losers;
            return View(winners);
        }
    }
}