﻿

using Gitpilot.Enums;

namespace Gitpilot.Models
{
    public class BaseResponse
    {

        public BaseResponse()
        {
            var sucess = ErrorEnum.Sucess;
            ErrorCode = (int)sucess;
            ErrorMessage = sucess.ToString();
        }

        public BaseResponse(ErrorEnum cacheReloadFailed)
        {
            ErrorCode = (int)cacheReloadFailed;
            ErrorMessage = cacheReloadFailed.ToString();
        }

        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}