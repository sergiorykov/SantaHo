using System.Collections.Generic;

namespace SantaHo.Domain.IncomingLetters
{
    public sealed class Letter
    {
        public Letter()
        {
            Wishes = new List<string>();
        }

        public string Name { get; set; }
        public List<string> Wishes { get; set; }
    }
}