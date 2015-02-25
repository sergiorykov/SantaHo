using System;
using System.Net;
using System.ServiceModel.Web;
using NLog;
using SantaHo.Core.Exceptions;

namespace SantaHo.FrontEnd.Service.Hosts.Executors
{
    public static class HostExecutor
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void Execute(Action action)
        {
            Execute(() =>
            {
                action();
                return true;
            });
        }

        public static TResult Execute<TResult>(Func<TResult> action)
        {
            try
            {
                return action();
            }
            catch (QueueUnavailableException e)
            {
                Logger.Warn(e);
                throw new WebFaultException(HttpStatusCode.InternalServerError);
            }
            catch (ValidationException e)
            {
                Logger.Warn(e);
                throw new WebFaultException(HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                Logger.Warn(e);
                throw new WebFaultException(HttpStatusCode.InternalServerError);
            }
        }
    }
}
