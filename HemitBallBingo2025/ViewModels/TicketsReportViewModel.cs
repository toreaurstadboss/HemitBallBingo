using Data.Models;

namespace HemitBallBingo2025.ViewModels
{

    public class TicketsReportViewModel
    {
        public string DrawName { get; set; }
        public DateTime Created { get; set; }
        public List<Ticket> Tickets { get; set; } = new();
    }

}
