using Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace HemitBallBingo2025.ViewModels
{
    [BindProperties]
    public class TicketsViewModel
    {
        public int DrawId { get; set; }
        public LotteryDraw? LotteryDraw { get; set; }

        // Allow up to 10 tickets
        public List<TicketInputModel> Tickets { get; set; }

        public TicketsViewModel()
        {
            Tickets = new List<TicketInputModel>(); 

            // Initialize with 10 empty slots
            for (int i = 0; i < 10; i++)
            {
                Tickets.Add(new TicketInputModel
                {
                    OwnerName = string.Empty
                });
            }
        }
    }

    [BindProperties]
    public class TicketInputModel
    {
        public string OwnerName { get; set; }
    }

}
