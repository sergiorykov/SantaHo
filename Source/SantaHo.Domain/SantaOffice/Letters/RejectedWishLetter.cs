namespace SantaHo.Domain.SantaOffice.Letters
{
    public sealed class RejectedWishLetter : Letter
    {
        public string Wish { get; set; }
        public string Reason { get; set; }
    }
}