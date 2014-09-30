using System;
using System.Collections.Generic;
using System.Linq;
using SantaHo.Core.Extensions;
using SantaHo.ServiceContracts.Letters;

namespace GoodBoy.Bot.Tasks
{
    public sealed class LetterProvider
    {
        private static readonly Dictionary<int, string> Wishes = GetWishesPool()
            .Select((x, i) => new KeyValuePair<int, string>(i, x))
            .ToDictionary(x => x.Key, x => x.Value);

        private readonly Random _random;

        public LetterProvider()
        {
            _random = new Random(Environment.TickCount);
        }

        private static IEnumerable<string> GetWishesPool()
        {
            return new List<string>
            {
                "Car",
                "Girls",
                "Villa",
                "Knife",
                "Gun",
                "Rock'n'Roll",
            };
        }

        public WishListLetterRequest Create()
        {
            return new WishListLetterRequest
            {
                Name = "Good boy {0}".F(Guid.NewGuid().ToString()),
                Wishes = GetCoupleOfWishes()
            };
        }

        private List<string> GetCoupleOfWishes()
        {
            int howMany = _random.Next(Wishes.Count - 1);
            return Enumerable.Range(1, howMany)
                .Select(x => _random.Next(x))
                .Select(x => Wishes[x])
                .ToList();
        }
    }
}