namespace ToDoList.Models
{
    public class ResponseHandler
    {
        public static ApiResponse GetAppResponse(ResponseType type, object? contract)
        {
            ApiResponse response = new ApiResponse()
            {
                ResponseData = contract
            };

            switch (type)
            {
                case ResponseType.Success:
                    response.Code = "0";
                    response.Message = "Success";                       
                    break;
                case ResponseType.NotFound:
                    response.Code = "2";
                    response.Message = "No record avaliable";
                    break;
                default:
                    break;
            }

            return response;
        }

        public static ApiResponse GetExceptionResponse(Exception ex)
        {
            ApiResponse response = new ApiResponse();
            response.Code = "1";
            response.ResponseData = ex.Message;

            return response;
        }
    }
}
