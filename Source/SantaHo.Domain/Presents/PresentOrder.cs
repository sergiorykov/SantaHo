using System;
using System.Collections.Generic;
using SantaHo.Domain.SantaOffice;

namespace SantaHo.Domain.Presents
{
    public class PresentOrder
    {
        public PresentOrder()
        {
            ToProduce = new List<ToyOrder>();
            Rejected = new List<RejectedWish>();
        }

        public Guid Id { get; set; }

        public string To { get; set; }

        public List<ToyOrder> ToProduce { get; set; }
        public List<RejectedWish> Rejected { get; set; }
    }
}