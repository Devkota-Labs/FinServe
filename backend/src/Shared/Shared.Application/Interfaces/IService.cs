using Shared.Application.Results;

namespace Shared.Application.Interfaces;

public interface IService<TReturnDto, TCreateDto, TUpdateDto>
{
    Task<Result<ICollection<TReturnDto>>> GetAllAsync(CancellationToken cancellationToken);
    Task<Result<TReturnDto?>> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Result<TReturnDto>> CreateAsync(TCreateDto dto, CancellationToken cancellationToken);
    Task<Result<TReturnDto>> UpdateAsync(int id, TUpdateDto dto, CancellationToken cancellationToken);
    Task<Result<TReturnDto>> DeleteAsync(int id, CancellationToken cancellationToken);
}