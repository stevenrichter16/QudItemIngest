namespace Application.Dto;

public class WeaponDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public string WeaponType { get; set; } = string.Empty;

    public string BaseDamage { get; set; } = string.Empty;
    public string Skill { get; set; } = string.Empty;
}