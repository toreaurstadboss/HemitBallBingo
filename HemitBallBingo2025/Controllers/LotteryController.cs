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

        public async Task<IActionResult> Intro(int drawId)
        {
            var draw = await _context.LotteryDraws
                .Include(d => d.Tickets)
                .FirstOrDefaultAsync(d => d.Id == drawId);
            if (draw == null) return NotFound();

            var model = new IntroViewModel
            {
                DrawId = drawId,
                DrawName = draw.Name,
                DrawDate = draw.Created,
                ParticipantCount = draw.Tickets.Count
            };
            return View(model);
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
                .Where(t => t.LotteryDrawId == drawId && !t.IsDrawn)
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

            // Fetch drawn tickets for prizes
            var drawnTickets = await _context.Tickets
                .Where(t => t.LotteryDrawId == drawId && t.IsDrawn && t.PrizeNumber != null)
                .ToListAsync();
            viewModel.ThirdPrize = drawnTickets.FirstOrDefault(t => t.PrizeNumber == 3);
            viewModel.SecondPrize = drawnTickets.FirstOrDefault(t => t.PrizeNumber == 2);
            viewModel.FirstPrize = drawnTickets.FirstOrDefault(t => t.PrizeNumber == 1);

            //Decid which image to show to end-user based on drawn tickets and prices

            int drawCount = (TempData["DrawCount"] as int? ?? 0) + 1;
            if (drawCount > 15)
            {
                drawCount = 1;
            }
            TempData["DrawCount"] = drawCount;

            // Decide image based on drawCount
            string imagePath = drawCount switch
            {
                1 => "../images/stairs.png",
                6 or 11 => "../images/player100.png",
                2 => "../images/player390.png",
                7 or 12 => "../images/piggybank.png",
                13 => "../images/player456andfrontman.png",
                3 or 8 => "../images/scared.png",
                4 or 9 or 14 => "../images/thebully.png",
                5 or 10 or 15 => "../images/mommasboy.png",
                _ => "../images/stairs.png"           
            };

            if (viewModel.ThirdPrize != null)
            {
                imagePath = "../images/oldguy.png";
            }
            if (viewModel.SecondPrize != null)
            {
                imagePath = "../images/thanos.png";
            }
            if (viewModel.FirstPrize != null)
            {
                imagePath = "../images/herosmilingplayer456.png";
            }

            TempData["DrawnTicketImage"] = imagePath;

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

            // Convert to positive integer
            int randomInt = BitConverter.ToInt32(randomBytes, 0) & int.MaxValue;

            // Get index within range
            int index = randomInt % tickets.Count;

            var drawnTicket = tickets[index];


            // Prepare message
            string message;
            if (tickets.Count <= 3)
            {
                // Prize logic based on remaining tickets
                message = tickets.Count switch
                {
                    3 => $"🏅 Winner 3rd Prize! {Environment.NewLine} {drawnTicket.OwnerName} (Ticket #{drawnTicket.TicketNumber})",
                    2 => $"🥈 Winner 2nd Prize! {Environment.NewLine} {drawnTicket.OwnerName} (Ticket #{drawnTicket.TicketNumber})",
                    1 => $"🥇 Winner 1st Prize! {Environment.NewLine} {drawnTicket.OwnerName} (Ticket #{drawnTicket.TicketNumber})",
                    _ => $"Better Luck Next Time: {Environment.NewLine} {drawnTicket.OwnerName} (Ticket #{drawnTicket.TicketNumber})"
                };
                drawnTicket.PrizeNumber = tickets.Count; // 3 for third, 2 for second, 1 for first
            }
            else
            {
                message = $"Bedre lykke neste gang:  " + $"{Environment.NewLine}" + $"{drawnTicket.OwnerName} (Ticket #{drawnTicket.TicketNumber})";
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

            List<string> registeredTicketOwnerNames = _context.Tickets
                .Where(t => t.LotteryDrawId == model.DrawId)
                .Select(t => t.OwnerName)
                .Distinct()
                .ToList();

            // Collect valid tickets (non-emtpy ticket owner names / participants of the Bingo draw = lodd trekningen)
            var ticketsToAdd = new List<Ticket>();

            bool isAnyUserEntered = false; //this flag tells if any user is added to the Tickets 

            if (!string.IsNullOrWhiteSpace(model.TicketsRaw?.Trim()))
            {
                var rawParticipants = model.TicketsRaw.Split(new[] { '\r', '\n', ',', ':' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var participant in rawParticipants)
                {
                    string participantName = participant.Trim();

                    if (!string.IsNullOrWhiteSpace(participant))
                    {
                        if (registeredTicketOwnerNames.Contains(participantName))
                        {
                            continue; // Skip if this participant already has a ticket
                        }

                        maxTicketNumber++;
                        int ticketNumber = maxTicketNumber;                     

                        ticketsToAdd.Add(new Ticket
                        {
                            OwnerName = participantName,
                            TicketNumber = ticketNumber,
                            LotteryDrawId = model.DrawId
                        });
                        isAnyUserEntered = true;
                    }
                }
            }

            if (!isAnyUserEntered)
            {

                foreach (var ticketInput in model.Tickets)
                {
                    if (!string.IsNullOrWhiteSpace(ticketInput.OwnerName))
                    {
                        maxTicketNumber++; // increment for next available number
                        int ticketNumber = maxTicketNumber;

                        string participantName = ticketInput.OwnerName.Trim();

                        if (!registeredTicketOwnerNames.Contains(participantName))
                        {
                            continue; // Skip if this participant already has a ticket
                        }

                        ticketsToAdd.Add(new Ticket
                        {
                            OwnerName = participantName,
                            TicketNumber = ticketNumber,
                            LotteryDrawId = model.DrawId
                        });
                        isAnyUserEntered = true;
                    }
                }
            }

            // Block if no tickets entered
            if (!isAnyUserEntered)
            {
                ModelState.AddModelError("", "Please enter at least one Owner Name to participate in the Lottery Draw.");
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