namespace OrderFlow.Domain.Exceptions;

public class NotFoundException : DomainException
{
    public NotFoundException(string entity, Guid id)
        : base($"{entity} com ID '{id}' não foi encontrado(a).")
    {
    }
}