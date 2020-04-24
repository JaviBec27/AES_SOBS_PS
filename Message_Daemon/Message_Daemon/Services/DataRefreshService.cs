using Message_Daemon.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Message_Daemon.Services
{
    public class DataRefreshService : HostedService
    {
        private readonly RandomStringProvider _randomStringProvider;

        public DataRefreshService(RandomStringProvider randomStringProvider)
        {
            _randomStringProvider = randomStringProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await _randomStringProvider.UpdateString(cancellationToken);
                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }
        }
    }
}
