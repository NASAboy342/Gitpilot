using Gitpilot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitpilot.Queues
{
    public class MessageQueue : QueueBase<BaseResponse>
    {
        /// <summary>
        /// Recommended to use for sending error messages.
        /// </summary>
        /// <param name="message"></param>
        public void Send(BaseResponse message)
        {
            this.Enqueue(new BaseResponse
            {
                CreatedTime = DateTime.Now,
                ErrorCode = message.ErrorCode,
                ErrorMessage = message.ErrorMessage
            });
        }
    }
}
