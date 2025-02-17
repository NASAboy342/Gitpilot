using Gitpilot.Models;
using Gitpilot.Queues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitpilot.Helpers
{
    public class OnLoading : IDisposable
    {
        private string _loadingHash;
        private readonly OnloadingQueue _onloadingQueue;
        public OnLoading(OnloadingQueue onloadingQueue)
        {
            _onloadingQueue = onloadingQueue;
            CreateLoadingRequest();

        }

        private async Task CreateLoadingRequest()
        {
            _loadingHash = Guid.NewGuid().ToString();
            _onloadingQueue.Enqueue(new LoadingInfo(_loadingHash));
        }

        private async Task CreateStopLoadingRequest()
        {
            _onloadingQueue.Enqueue(new LoadingInfo(_loadingHash));
        }


        public void Dispose()
        {
            CreateStopLoadingRequest();
        }
    }
}
