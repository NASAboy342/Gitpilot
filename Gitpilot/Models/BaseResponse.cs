

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
            CreatedTime = DateTime.Now;
        }

        public BaseResponse(Exception ex)
        {
            ErrorCode = (int)ErrorEnum.GeneralError;
            ErrorMessage = ex.Message;
            CreatedTime = DateTime.Now;
        }

        public BaseResponse(ErrorEnum cacheReloadFailed)
        {
            ErrorCode = (int)cacheReloadFailed;
            ErrorMessage = cacheReloadFailed.ToString();
            CreatedTime = DateTime.Now;
        }

        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;
    }
}