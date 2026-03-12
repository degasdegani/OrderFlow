using AutoMapper;
using Microsoft.Extensions.Logging;
using OrderFlow.Application.DTOs.Common;
using OrderFlow.Application.DTOs.Stock;
using OrderFlow.Application.Interfaces;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Enums;
using OrderFlow.Domain.Exceptions;
using OrderFlow.Domain.Interfaces;

namespace OrderFlow.Application.Services;

public class StockService : IStockService
{
    private readonly IStockMovementRepository _stockRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<StockService> _logger;

    public StockService(IStockMovementRepository stockRepository, IProductRepository productRepository,
        IUnitOfWork unitOfWork, IMapper mapper, ILogger<StockService> logger)
    {
        _stockRepository = stockRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedResultDto<StockMovementResponseDto>> GetMovementsAsync(
        int page, int pageSize, Guid? productId, MovementType? type, DateTime? startDate, DateTime? endDate)
    {
        var (items, totalCount) = await _stockRepository.GetPagedAsync(page, pageSize, productId, type, startDate, endDate);

        return new PagedResultDto<StockMovementResponseDto>
        {
            Items = _mapper.Map<List<StockMovementResponseDto>>(items),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<ApiResponseDto<StockMovementResponseDto>> CreateMovementAsync(
        CreateStockMovementDto dto, Guid userId)
    {
        var product = await _productRepository.GetByIdAsync(dto.ProductId)
            ?? throw new NotFoundException("Produto", dto.ProductId);

        var previousQuantity = product.StockQuantity;
        int newQuantity;

        switch (dto.MovementType)
        {
            case MovementType.In:
                newQuantity = previousQuantity + dto.Quantity;
                break;

            case MovementType.Out:
                if (product.StockQuantity < dto.Quantity)
                    throw new BusinessRuleException(
                        $"Estoque insuficiente. Disponível: {product.StockQuantity}, Solicitado: {dto.Quantity}.");
                newQuantity = previousQuantity - dto.Quantity;
                break;

            case MovementType.Adjustment:
                newQuantity = dto.Quantity;
                break;

            default:
                throw new BusinessRuleException("Tipo de movimentação inválido.");
        }

        product.StockQuantity = newQuantity;
        _productRepository.Update(product);

        var movement = new StockMovement
        {
            ProductId = dto.ProductId,
            MovementType = dto.MovementType,
            Quantity = dto.Quantity,
            PreviousQuantity = previousQuantity,
            NewQuantity = newQuantity,
            Reason = dto.Reason,
            UserId = userId
        };

        await _stockRepository.AddAsync(movement);
        await _unitOfWork.CommitAsync();

        _logger.LogInformation("Movimentação de estoque registrada. ID: {Id}", movement.Id);

        return ApiResponseDto<StockMovementResponseDto>.Success(
            _mapper.Map<StockMovementResponseDto>(movement), "Movimentação registrada com sucesso.");
    }
}