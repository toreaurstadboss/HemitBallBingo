namespace HemitBallBingo2025.ViewModels
{
    public class IntroViewModel
    {
        public int DrawId { get; set; }
        public string DrawName { get; set; } = string.Empty;
        public DateTime DrawDate { get; set; }
        public int ParticipantCount { get; set; }
        public int PrizeAmount => ParticipantCount * 50;
    }
}
