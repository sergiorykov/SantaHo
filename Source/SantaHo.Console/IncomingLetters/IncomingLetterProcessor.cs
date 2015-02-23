using System.Diagnostics.Contracts;
using SantaHo.Core.Processing;
using SantaHo.Domain.Letters;
using SantaHo.Domain.Presents;
using SantaHo.Domain.SantaOffice;

namespace SantaHo.SantaOffice.Service.IncomingLetters
{
    public sealed class IncomingLetterProcessor
    {
        private readonly Santa _santa;
        private readonly IPresentOrderProcessor _presentOrderProcessor;

        public IncomingLetterProcessor(Santa santa, IPresentOrderProcessor presentOrderProcessor)
        {
            Contract.Requires(santa != null);
            Contract.Requires(presentOrderProcessor != null);

            _santa = santa;
            _presentOrderProcessor = presentOrderProcessor;
        }

        public void Process(Letter letter)
        {
            Contract.Requires(letter != null);

            PresentOrder order = _santa.Read(letter);
            _presentOrderProcessor.Process(order);
        }
    }
}