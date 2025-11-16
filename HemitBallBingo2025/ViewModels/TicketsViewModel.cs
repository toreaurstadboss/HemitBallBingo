using Data.Models;

namespace HemitBallBingo2025.ViewModels
{

    public class TicketsViewModel
    {
        public int DrawId { get; set; }
        public LotteryDraw? LotteryDraw { get; set; }

        // Allow up to 10 tickets
        public List<TicketInputModel> Tickets { get; set; } = new List<TicketInputModel>();

        public TicketsViewModel()
        {
            // Initialize with 10 empty slots
            for (int i = 0; i < 10; i++)
            {
                Tickets.Add(new TicketInputModel());
            }
        }
    }

    public class TicketInputModel
    {
        public string? OwnerName { get; set; }
    }

}
