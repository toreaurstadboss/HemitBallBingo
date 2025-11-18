using Data.Models;

namespace HemitBallBingo2025.ViewModels
{

    public class DrawViewModel
    {
        public int DrawId { get; set; }
        public string DrawName { get; set; } = string.Empty;
        public DateTime DrawDate { get; set; }

        public List<Ticket> Tickets { get; set; } = new List<Ticket>();

        // Optional: For showing the last drawn ticket
        public string? LastDrawnMessage { get; set; }

        public Ticket? ThirdPrize { get; set; }
        public Ticket? SecondPrize { get; set; }
        public Ticket? FirstPrize { get; set; }

    }

}
