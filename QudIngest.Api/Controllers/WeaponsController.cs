using Application.Dto;
using Application.Features.IngestWeapon;
using Microsoft.AspNetCore.Mvc;

namespace QudIngest.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeaponsController: ControllerBase
{
    private IngestWeaponCommandHandler _commandHandler;

    public WeaponsController(IngestWeaponCommandHandler commandHandler)
    {
        _commandHandler = commandHandler;
    }

    [HttpPost("ingest")]
    public async Task<IActionResult> Ingest([FromBody]WeaponDto weaponDto, CancellationToken cancellationToken)
    {
        var id = await _commandHandler.HandleAsync(new IngestWeaponCommand(weaponDto), cancellationToken);
        return Ok(new { id });
    }
}