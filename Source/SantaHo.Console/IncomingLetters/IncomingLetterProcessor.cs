using System;
using System.Diagnostics.Contracts;
using SantaHo.Domain.Presents;
using SantaHo.Domain.SantaOffice;
using SantaHo.Domain.SantaOffice.Letters;
using SantaHo.SantaOffice.Service.Presents;

namespace SantaHo.SantaOffice.Service.IncomingLetters
{
    public sealed class IncomingLetterProcessor
    {
        private readonly IPresentOrderProcessor _presentOrderProcessor;
        private readonly Santa _santa;

        public IncomingLetterProcessor(Santa santa, IPresentOrderProcessor presentOrderProcessor)
        {
            Contract.Requires(santa != null);
            Contract.Requires(presentOrderProcessor != null);

            _santa = santa;
            _presentOrderProcessor = presentOrderProcessor;
        }

        public void Process(IncomingChildLetter letter)
        {
            Contract.Requires(letter != null);

            PresentOrder order = _santa.Read(letter);
            _presentOrderProcessor.Process(order);
        }
    }
}
