using Sentinel.Helpers;
namespace Sentinel.Helpers
{
    public class ApiResponseHelper
    {
        public ApiResponse<T> Success<T>(T result, string responseMessage)
        {
            return new ApiResponse<T>
            {
                Status = "SUCCESS",
                Response = responseMessage,
                Result = result
            };
        }

        public ApiResponse<T> SuccessList<T>(List<T> resultList, string responseMessage)
        {
            return new ApiResponse<T>
            {
                Status = "SUCCESS",
                Response = responseMessage,
                ResponseList = resultList
            };
        }

        public ApiResponse<T> SuccessData<T>(object data, string responseMessage)
        {
            return new ApiResponse<T>
            {
                Status = "SUCCESS",
                Response = responseMessage,
                Data = data
            };
        }

        public ApiResponse<T> Error<T>(string responseMessage)
        {
            return new ApiResponse<T>
            {
                Status = "ERROR",
                Response = responseMessage
            };
        }
    }
}
