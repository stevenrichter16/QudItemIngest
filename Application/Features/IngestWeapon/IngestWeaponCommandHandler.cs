using Application.Dto;
using Domain.Abstractions;
using Domain.Entities;

namespace Application.Features.IngestWeapon;

public class IngestWeaponCommandHandler
{
    private readonly IWeaponRepository _repository;

    public IngestWeaponCommandHandler(IWeaponRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> HandleAsync(IngestWeaponCommand command,  CancellationToken cancellationToken)
    {
        var dto = command.WeaponDto;
        var weapon = new Weapon()
        {
            Name = dto.Name,
            WeaponType = dto.WeaponType,
            BaseDamage = dto.BaseDamage,
            Skill = dto.Skill,
        };

        await _repository.AddAsync(weapon);
        return weapon.Id;
    }
}