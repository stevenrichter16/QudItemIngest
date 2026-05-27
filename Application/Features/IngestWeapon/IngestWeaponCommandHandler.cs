using Application.Dto;
using Domain.Abstractions;
using Domain.Entities;

namespace Application.Features.IngestWeapon;

public class IngestWeaponCommandHandler(IWeaponRepository repository)
{
    public async Task<Guid> HandleAsync(IngestWeaponCommand command,  CancellationToken cancellationToken)
    {
        var dto = command.WeaponDto;
        var weapon = new Weapon
        {
            Name = dto.Name,
            WeaponType = dto.WeaponType,
            BaseDamage = dto.BaseDamage,
            Skill = dto.Skill
        };

        await repository.AddAsync(weapon, cancellationToken);
        return weapon.Id;
    }
}