﻿using System;
using System.Collections.Generic;
using System.Linq;
using SantaHo.Core.Extensions;
using SantaHo.FrontEnd.ServiceContracts.Letters;

namespace GoodBoy.Bot.Providers
{
    public sealed class WishRequestProvider
    {
        private readonly Random _random;

        public WishRequestProvider()
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
                "Rock'n'Roll"
            };
        }

        public WishListLetterRequest Create()
        {
            return new WishListLetterRequest
            {
                Name = "Good boy {0}".FormatWith(Guid.NewGuid().ToString()),
                Wishes = GetCoupleOfWishes()
            };
        }

        private List<string> GetCoupleOfWishes()
        {
            int howMany = _random.Next(1, Wishes.Count);
            return Enumerable.Range(1, howMany)
                             .Select(x => _random.Next(Wishes.Count - 1))
                             .Select(x => Wishes[x])
                             .ToList();
        }

        private static readonly Dictionary<int, string> Wishes = GetWishesPool()
            .Select((x, i) => new KeyValuePair<int, string>(i, x))
            .ToDictionary(x => x.Key, x => x.Value);
    }
}
