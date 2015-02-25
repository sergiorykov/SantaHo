using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SantaHo.Core.Exceptions;

namespace SantaHo.Domain.SantaOffice.Letters
{
    public sealed class IncomingChildLetter : Letter
    {
        public IncomingChildLetter()
        {
            Wishes = new List<string>();
        }

        public List<string> Wishes { get; private set; }

        public void Validate()
        {
            Contract.Requires<ValidationException>(!string.IsNullOrWhiteSpace(From), "From");
            Contract.Requires<ValidationException>(Wishes.Count > 0, "Wishes");
        }
    }
}