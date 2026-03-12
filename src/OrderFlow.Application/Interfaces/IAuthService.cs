using OrderFlow.Application.DTOs.Auth;
using OrderFlow.Application.DTOs.Common;

namespace OrderFlow.Application.Interfaces;

public interface IAuthService
{
    Task<ApiResponseDto<TokenResponseDto>> LoginAsync(LoginRequestDto dto);
    Task<ApiResponseDto<string>> RegisterAsync(RegisterRequestDto dto);
}