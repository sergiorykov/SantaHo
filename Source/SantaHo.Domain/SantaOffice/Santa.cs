using System;
using System.Linq;
using System.Threading;
using NLog;
using SantaHo.Domain.Presents;
using SantaHo.Domain.SantaOffice.Letters;

namespace SantaHo.Domain.SantaOffice
{
    public class Santa
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public PresentOrder Read(IncomingChildLetter letter)
        {
            Logger.Info("Santa is reading letter from: {0}", letter.From);
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
                    ToyCategory = x.ToLowerInvariant(),
                    Wish = x
                })
                .ToList();

            return order;
        }
    }
}