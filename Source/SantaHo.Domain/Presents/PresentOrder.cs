﻿using System;
using System.Collections.Generic;
using SantaHo.Domain.SantaOffice;
using SantaHo.Domain.SantaOffice.Letters;

namespace SantaHo.Domain.Presents
{
    public class PresentOrder
    {
        public PresentOrder()
        {
            ToProduce = new List<ToyOrder>();
            Rejected = new List<RejectedWishLetter>();
        }

        public Guid Id { get; set; }
        public string To { get; set; }
        public List<ToyOrder> ToProduce { get; set; }
        public List<RejectedWishLetter> Rejected { get; set; }
    }
}
