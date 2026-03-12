using AutoMapper;
using Microsoft.Extensions.Logging;
using OrderFlow.Application.DTOs.Category;
using OrderFlow.Application.DTOs.Common;
using OrderFlow.Application.Interfaces;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Exceptions;
using OrderFlow.Domain.Interfaces;

namespace OrderFlow.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(ICategoryRepository repository, IUnitOfWork unitOfWork,
        IMapper mapper, ILogger<CategoryService> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedResultDto<CategoryResponseDto>> GetAllAsync(
        int page, int pageSize, string? search)
    {
        var (items, totalCount) = await _repository.GetPagedAsync(page, pageSize, search);

        return new PagedResultDto<CategoryResponseDto>
        {
            Items = _mapper.Map<List<CategoryResponseDto>>(items),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<ApiResponseDto<CategoryResponseDto>> GetByIdAsync(Guid id)
    {
        var category = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException("Categoria", id);

        return ApiResponseDto<CategoryResponseDto>.Success(_mapper.Map<CategoryResponseDto>(category));
    }

    public async Task<ApiResponseDto<CategoryResponseDto>> CreateAsync(CreateCategoryDto dto)
    {
        var category = _mapper.Map<Category>(dto);
        await _repository.AddAsync(category);
        await _unitOfWork.CommitAsync();

        _logger.LogInformation("Categoria criada com sucesso. ID: {Id}", category.Id);

        return ApiResponseDto<CategoryResponseDto>.Success(
            _mapper.Map<CategoryResponseDto>(category), "Categoria criada com sucesso.");
    }

    public async Task<ApiResponseDto<CategoryResponseDto>> UpdateAsync(Guid id, UpdateCategoryDto dto)
    {
        var category = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException("Categoria", id);

        category.Name = dto.Name;
        category.Description = dto.Description;

        _repository.Update(category);
        await _unitOfWork.CommitAsync();

        _logger.LogInformation("Categoria atualizada com sucesso. ID: {Id}", category.Id);

        return ApiResponseDto<CategoryResponseDto>.Success(
            _mapper.Map<CategoryResponseDto>(category), "Categoria atualizada com sucesso.");
    }

    public async Task DeleteAsync(Guid id)
    {
        var category = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException("Categoria", id);

        _repository.Delete(category);
        await _unitOfWork.CommitAsync();

        _logger.LogInformation("Categoria removida com sucesso. ID: {Id}", id);
    }
}