using Gitpilot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitpilot.Services.Interfaces
{
    public interface IAccountService
    {
        Task<BaseResponse> AddAccountProfile(RemoteAccountInfo remoteAccountInfo);
    }
}
