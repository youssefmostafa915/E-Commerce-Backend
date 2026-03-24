using System.Net;

namespace E_Commerce.Domain.Shared; 
public class BaseResponse<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public string ErrorCode { get; set; } = string.Empty;
    public HttpStatusCode StatusCode { get; set; }
    public T? Data { get; set; }

    public static BaseResponse<T> Ok(T data, string message = "Success", HttpStatusCode statusCode = HttpStatusCode.OK) 
        => new() { IsSuccess = true, Data = data, Message = message, StatusCode = statusCode };

    public static BaseResponse<T> Fail(string errorCode, string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest) 
        => new() { IsSuccess = false, ErrorCode = errorCode, Message = message, StatusCode = statusCode };
}