using System;
using System.Net;
using System.ServiceModel.Web;
using AutoMapper;
using Nelibur.Core.Extensions;
using Nelibur.ServiceModel.Services.Operations;
using NLog;
using SantaHo.Domain.IncomingLetters;
using SantaHo.ServiceContracts.Letters;

namespace SantaHo.ServiceHosts.Processors
{
    public class WishListProcessor : IPostOneWay<WishListLetterRequest>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IIncomingLettersEnqueuer _enqueuer;

        public WishListProcessor(IIncomingLettersEnqueuer enqueuer)
        {
            _enqueuer = enqueuer;
        }

        public void PostOneWay(WishListLetterRequest request)
        {
            request.ToOption()
                .Where(x => !string.IsNullOrWhiteSpace(x.Name))
                .Where(x => x.Wishes.Count > 0)
                .ThrowOnEmpty(() => new WebFaultException(HttpStatusCode.BadRequest));

            Execute(() => Enqueue(request));
        }

        private void Enqueue(WishListLetterRequest request)
        {
            Letter letter = Mapper.Map<WishListLetterRequest, Letter>(request);
            _enqueuer.Enque(letter);
        }

        private void Execute(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                Logger.Warn(e);
                throw new WebFaultException(HttpStatusCode.InternalServerError);
            }
        }
    }
}