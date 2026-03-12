namespace OrderFlow.Application.DTOs.Common;

public class ApiResponseDto<T>
{
    public bool Succeeded { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = new();

    public static ApiResponseDto<T> Success(T data, string message = "Operação realizada com sucesso.")
    {
        return new ApiResponseDto<T>
        {
            Succeeded = true,
            Message = message,
            Data = data
        };
    }

    public static ApiResponseDto<T> Failure(string error)
    {
        return new ApiResponseDto<T>
        {
            Succeeded = false,
            Errors = new List<string> { error }
        };
    }

    public static ApiResponseDto<T> Failure(List<string> errors)
    {
        return new ApiResponseDto<T>
        {
            Succeeded = false,
            Errors = errors
        };
    }
}