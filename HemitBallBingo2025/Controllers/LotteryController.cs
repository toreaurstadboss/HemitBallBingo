using Data;
using Data.Models;
using HemitBallBingo2025.ViewModels;
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
            var model = new LotteryDrawViewModelOverview();
            model.LotteryDraws.AddRange(draws);
            return View(model);
        }       

        [HttpGet]
        public IActionResult Create()
        {
            var model = new LotteryDrawCreateViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(LotteryDrawCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var draw = new LotteryDraw
                {
                    Name = model.Name,
                    Created = model.Created
                };

                _context.LotteryDraws.Add(draw);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Register(int drawId)
        {
            var draw = await _context.LotteryDraws.FindAsync(drawId);

            var tickets = new TicketsViewModel
            {
                DrawId = drawId,
                LotteryDraw = draw
            };
            return View(tickets);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterTickets(TicketsViewModel model)
        {
            // Validate DrawId
            var draw = await _context.LotteryDraws.FindAsync(model.DrawId);
            if (draw == null)
            {
                return NotFound();
            }

            // Get the current max TicketNumber for this draw
            int maxTicketNumber = await _context.Tickets
                .Where(t => t.LotteryDrawId == model.DrawId)
                .Select(t => t.TicketNumber)
                .DefaultIfEmpty(0)
                .MaxAsync();

            // Collect valid tickets (OwnerName not empty)
            var ticketsToAdd = new List<Ticket>();
            foreach (var ticketInput in model.Tickets)
            {
                if (!string.IsNullOrWhiteSpace(ticketInput.OwnerName))
                {
                    maxTicketNumber++; // increment for next available number
                    int ticketNumber = maxTicketNumber;

                    ticketsToAdd.Add(new Ticket
                    {
                        OwnerName = ticketInput.OwnerName.Trim(),
                        TicketNumber = ticketNumber,
                        LotteryDrawId = model.DrawId
                    });
                }
            }

            // Block if no tickets entered
            if (!ticketsToAdd.Any())
            {
                ModelState.AddModelError("", "Please enter at least one Owner Name.");
                return View(model);
            }

            // Save tickets
            _context.Tickets.AddRange(ticketsToAdd);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}