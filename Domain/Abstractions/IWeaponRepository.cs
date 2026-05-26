using Domain.Entities;

namespace Domain.Abstractions;

public interface IWeaponRepository
{
    public Task AddAsync(Weapon weapon,  CancellationToken cancellationToken = default);
}