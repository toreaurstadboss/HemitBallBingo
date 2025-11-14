using Data;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HemitBallBingo2025.Controllers
{
    public class LotteryController : Controller
    {
        private readonly HemitBallbingoContext _context;

        public LotteryController(HemitBallbingoContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var draws = await _context.LotteryDraws.Include(d => d.Tickets).ToListAsync();
            return View(draws);
        }

        [HttpGet]
        public IActionResult Register(int drawId)
        {
            ViewBag.DrawId = drawId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(int drawId, string name, int ticketNumber)
        {
            var draw = await _context.LotteryDraws.FindAsync(drawId);
            if (draw != null && ticketNumber >= 1 && ticketNumber <= 300)
            {
                _context.Tickets.Add(new Ticket { OwnerName = name, TicketNumber = ticketNumber, LotteryDrawId = drawId });
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}