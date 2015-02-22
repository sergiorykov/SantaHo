using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SantaHo.Core.Exceptions;

namespace SantaHo.Domain.IncomingLetters
{
    public sealed class Letter
    {
        public Letter()
        {
            Wishes = new List<string>();
        }

        public string From { get; set; }

        public List<string> Wishes { get; private set; }

        public void Validate()
        {
            Contract.Requires<ValidationException>(!string.IsNullOrWhiteSpace(From), "From");
            Contract.Requires<ValidationException>(Wishes.Count > 0, "Wishes");
        }
    }
}