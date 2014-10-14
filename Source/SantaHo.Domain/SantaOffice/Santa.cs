using System;
using System.Linq;
using System.Threading;
using SantaHo.Domain.IncomingLetters;
using SantaHo.Domain.Presents;

namespace SantaHo.Domain.SantaOffice
{
    public class Santa
    {
        public PresentOrder Read(Letter letter)
        {
            Thread.Sleep(100);
            var order = new PresentOrder
            {
                Id = Guid.NewGuid(),
                To = letter.From,
            };

            order.ToProduce = letter.Wishes
                .Select(x => new ToyOrder
                {
                    PresentOrderId = order.Id,
                    ToyCategory = x,
                    Wish = x
                })
                .ToList();

            return order;
        }
    }
}