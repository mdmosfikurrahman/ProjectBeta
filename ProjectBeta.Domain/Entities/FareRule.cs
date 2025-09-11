namespace ProjectBeta.Domain.Entities;

public class FareRule
{
    public Guid Id { get; set; }
    public string? Cabin { get; set; }
    public string? FareBasis { get; set; }
    public decimal BaseFare { get; set; }
}
