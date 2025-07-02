using Microsoft.AspNetCore.Mvc;

namespace HmsBackend.DTOs
{
    public class StandardResponseDto<T> : ObjectResult
    {
        public StandardResponseDto(int status, string? message, T? data) : base(new { Message = message, Status = status, Data = data})
        {
            StatusCode = status;
        }
    }
}
