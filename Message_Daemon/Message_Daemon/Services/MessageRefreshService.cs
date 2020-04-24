using Message_Daemon.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Message_Daemon.Services
{
    public class MessageRefreshService : HostedService
    {
        private readonly MessageReceptionProvider _messageReceptionProvider;

        public MessageRefreshService(MessageReceptionProvider _messageReceptionProvider)
        {
            this._messageReceptionProvider = _messageReceptionProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await _messageReceptionProvider.GetMessages(cancellationToken);
                await Task.Delay(TimeSpan.FromSeconds(60),cancellationToken);
            }
        }
    }
}
