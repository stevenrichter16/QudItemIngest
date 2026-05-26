using Domain.Abstractions;
using Domain.Entities;
using Infrastructure.Context;

namespace Infrastructure.Repositories;

public class WeaponRepository: IWeaponRepository
{
    public readonly MongoContext _mongoContext;

    public WeaponRepository(MongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }

    public async Task AddAsync(Weapon weapon, CancellationToken cancellationToken = default)
    {
        await _mongoContext.Weapons.InsertOneAsync(weapon, cancellationToken: cancellationToken);
    }
}