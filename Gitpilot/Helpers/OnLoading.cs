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
        public OnLoading(OnloadingQueue onloadingQueue, string message = "")
        {
            _onloadingQueue = onloadingQueue;
            CreateLoadingRequest(message);

        }

        private async Task CreateLoadingRequest(string message)
        {
            _loadingHash = Guid.NewGuid().ToString();
            _onloadingQueue.Enqueue(new LoadingInfo(_loadingHash, message));
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
