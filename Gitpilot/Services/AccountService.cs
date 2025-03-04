using Gitpilot.Caches;
using Gitpilot.Enums;
using Gitpilot.Models;
using Gitpilot.Queues;
using Gitpilot.Repositories.Interfaces;
using Gitpilot.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitpilot.Services
{
    public class AccountService : IAccountService
    {
        private RemoteAccountInfoCache _remoteAccountInfoCache;
        private IGitpilotRepository _gitpilotRepository;
        private MessageQueue _messageQueue;
        public AccountService(RemoteAccountInfoCache remoteAccountInfoCache, IGitpilotRepository gitpilotRepository, MessageQueue messageQueue)
        {
            _remoteAccountInfoCache = remoteAccountInfoCache;
            _gitpilotRepository = gitpilotRepository;
            _messageQueue = messageQueue;
        }

        public async Task<BaseResponse> AddAccountProfile(RemoteAccountInfo remoteAccountInfo)
        {
            if (await IsProfileAlreadyExist(remoteAccountInfo))
            {
                return new BaseResponse(ErrorEnum.ProfileAlreadyExist);
            }
            await _gitpilotRepository.SaveRemoteAccountInfo(remoteAccountInfo);
            await _remoteAccountInfoCache.GetAllAsyns(true);
            return new BaseResponse();
        }

        private async Task<bool> IsProfileAlreadyExist(RemoteAccountInfo remoteAccountInfo)
        {
            var allProfiles = await _remoteAccountInfoCache.GetAllAsyns();
            var isDuplicated = allProfiles.Any(profile => profile.UserName.Equals(remoteAccountInfo.UserName, StringComparison.OrdinalIgnoreCase) && profile.PlatFormName.Equals(remoteAccountInfo.PlatFormName, StringComparison.OrdinalIgnoreCase));
            if (isDuplicated)
            {
                _messageQueue.Send(new BaseResponse(ErrorEnum.ProfileAlreadyExist));
                return true;
            }
            return false;
        }
    }
}
