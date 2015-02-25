namespace SantaHo.Domain.SantaOffice
{
    public abstract class RejectedWishLetter
    {
        public string To { get; set; }
        public string Wish { get; set; }
        public string Reason { get; set; }
    }
}