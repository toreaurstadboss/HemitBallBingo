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

        [HttpGet]
        public async Task<IActionResult> Draw(int drawId)
        {
            var draw = await _context.LotteryDraws.FindAsync(drawId);
            if (draw == null)
            {
                return NotFound();
            }

            var tickets = await _context.Tickets
                .Where(t => t.LotteryDrawId == drawId)
                .OrderBy(t => t.TicketNumber)
                .ToListAsync();

            var viewModel = new DrawViewModel
            {
                DrawId = draw.Id,
                DrawName = draw.Name,
                DrawDate = draw.Created,
                Tickets = tickets,
                LastDrawnMessage = TempData["DrawnTicket"] as string
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DrawNextTicket(int drawId)
        {
            // Fetch tickets for this draw
            var tickets = await _context.Tickets
                .Where(t => t.LotteryDrawId == drawId && !t.IsDrawn)
                .ToListAsync();

            if (!tickets.Any())
            {
                TempData["DrawnTicket"] = "No tickets available!";
                return RedirectToAction("Draw", new { drawId });
            }

            // Use a strong random generator
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            var randomBytes = new byte[4];
            rng.GetBytes(randomBytes);
            int index = BitConverter.ToInt32(randomBytes, 0) & int.MaxValue % tickets.Count;

            var drawnTicket = tickets[index];

            // Prepare message
            string message;
            if (tickets.Count <= 3)
            {
                // Prize logic based on remaining tickets
                message = tickets.Count switch
                {
                    3 => $"🏅 Winner 3rd Prize! {drawnTicket.OwnerName} (Ticket #{drawnTicket.TicketNumber})",
                    2 => $"🥈 Winner 2nd Prize! {drawnTicket.OwnerName} (Ticket #{drawnTicket.TicketNumber})",
                    1 => $"🥇 Winner 1st Prize! {drawnTicket.OwnerName} (Ticket #{drawnTicket.TicketNumber})",
                    _ => $"Better Luck Next Time: {drawnTicket.OwnerName} (Ticket #{drawnTicket.TicketNumber})"
                };
            }
            else
            {
                message = $"Better Luck Next Time: {drawnTicket.OwnerName} (Ticket #{drawnTicket.TicketNumber})";
            }

            TempData["DrawnTicket"] = message;

            // Later: mark ticket as drawn when you add IsDrawn flag
            drawnTicket.IsDrawn = true;
            await _context.SaveChangesAsync();

            return RedirectToAction("Draw", new { drawId });
        }

        [HttpGet]
        public async Task<IActionResult> TicketsReport(int drawId)
        {
            var draw = await _context.LotteryDraws
                .Include(d => d.Tickets)
                .FirstOrDefaultAsync(d => d.Id == drawId);

            if (draw == null) return NotFound();

            var model = new TicketsReportViewModel
            {
                DrawName = draw.Name,
                Created = draw.Created,
                Tickets = draw.Tickets.OrderBy(t => t.TicketNumber).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(TicketsViewModel model)
        {
            // Validate DrawId
            var draw = await _context.LotteryDraws.FindAsync(model.DrawId);
            if (draw == null)
            {
                return NotFound();
            }

            // Get the current max TicketNumber for this draw
            int maxTicketNumber = _context.Tickets
                .Where(t => t.LotteryDrawId == model.DrawId)
                .Select(t => t.TicketNumber).ToList()
                .DefaultIfEmpty(0)
                .Max();

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
                model.LotteryDraw = draw;
                model.DrawId = draw.Id;
                return View(model);
            }

            // Save tickets
            _context.Tickets.AddRange(ticketsToAdd);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}