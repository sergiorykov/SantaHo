using System;
using System.Collections.Generic;

namespace SantaHo.Domain.Presents
{
    public class Present
    {
        public Present()
        {
            Toys = new List<Toy>();
        }

        public Guid Id { get; set; }
        public string To { get; set; }
        public List<Toy> Toys { get; set; }
    }
}
