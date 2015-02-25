using System;

namespace SantaHo.Domain.SantaOffice
{
    public class ToyOrder
    {
        public Guid PresentOrderId { get; set; }
        public string ToyCategory { get; set; }
        public string Wish { get; set; }
    }
}
