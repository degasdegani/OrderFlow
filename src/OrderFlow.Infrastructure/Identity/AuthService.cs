using Microsoft.Extensions.Configuration;
using OrderFlow.Application.DTOs.Auth;
using OrderFlow.Application.DTOs.Common;
using OrderFlow.Application.Interfaces;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Enums;
using OrderFlow.Domain.Exceptions;
using OrderFlow.Domain.Interfaces;

namespace OrderFlow.Infrastructure.Identity;

public class AuthService : IAuthService
{
    private readonly IRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly TokenService _tokenService;
    private readonly IConfiguration _configuration;

    public AuthService(IRepository<User> userRepository, IUnitOfWork unitOfWork,
        TokenService tokenService, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _configuration = configuration;
    }

    public async Task<ApiResponseDto<TokenResponseDto>> LoginAsync(LoginRequestDto dto)
    {
        var users = await _userRepository.GetAllAsync();
        var user = users.FirstOrDefault(u => u.Email == dto.Email)
            ?? throw new BusinessRuleException("E-mail ou senha inválidos.");

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new BusinessRuleException("E-mail ou senha inválidos.");

        var token = _tokenService.GenerateToken(user);
        var expireHours = int.Parse(_configuration["Jwt:ExpireHours"] ?? "8");

        var response = new TokenResponseDto
        {
            Token = token,
            ExpiresIn = expireHours * 3600,
            UserName = user.FullName,
            Email = user.Email,
            Role = user.Role.ToString()
        };

        return ApiResponseDto<TokenResponseDto>.Success(response, "Login realizado com sucesso.");
    }

    public async Task<ApiResponseDto<string>> RegisterAsync(RegisterRequestDto dto)
    {
        var users = await _userRepository.GetAllAsync();

        if (users.Any(u => u.Email == dto.Email))
            throw new BusinessRuleException($"Já existe um usuário com o e-mail '{dto.Email}'.");

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = dto.Role
        };

        await _userRepository.AddAsync(user);
        await _unitOfWork.CommitAsync();

        return ApiResponseDto<string>.Success(user.Id.ToString(), "Usuário registrado com sucesso.");
    }
}