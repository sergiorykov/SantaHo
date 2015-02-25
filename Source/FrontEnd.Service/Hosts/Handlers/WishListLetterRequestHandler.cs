using System;
using System.Net;
using System.ServiceModel.Web;
using AutoMapper;
using Nelibur.ServiceModel.Services.Operations;
using Nelibur.Sword.Extensions;
using SantaHo.Domain.SantaOffice.Letters;
using SantaHo.FrontEnd.Service.Hosts.Executors;
using SantaHo.FrontEnd.Service.Queues;
using SantaHo.FrontEnd.ServiceContracts.Letters;

namespace SantaHo.FrontEnd.Service.Hosts.Handlers
{
    public sealed class WishListLetterRequestHandler : IPostOneWay<WishListLetterRequest>
    {
        private readonly IIncomingLetterQueue _queue;

        public WishListLetterRequestHandler(IIncomingLetterQueue queue)
        {
            _queue = queue;
        }

        public void PostOneWay(WishListLetterRequest request)
        {
            request.ToOption()
                   .ThrowOnEmpty(() => new WebFaultException(HttpStatusCode.BadRequest));

            HostExecutor.Execute(() => Enqueue(request));
        }

        private void Enqueue(WishListLetterRequest request)
        {
            IncomingChildLetter letter = Mapper.Map<WishListLetterRequest, IncomingChildLetter>(request);
            _queue.Enqueue(letter);
        }
    }
}
