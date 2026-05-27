using Application.Dto;
using Application.Features.IngestWeapon;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeaponsController(IngestWeaponCommandHandler commandHandler) : ControllerBase
{
    [HttpPost("ingest")]
    public async Task<IActionResult> Ingest([FromBody]WeaponDto weaponDto, CancellationToken cancellationToken)
    {
        var id = await commandHandler.HandleAsync(new IngestWeaponCommand(weaponDto), cancellationToken);
        return Ok(new { id });
    }
}