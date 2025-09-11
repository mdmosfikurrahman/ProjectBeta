namespace ProjectBeta.Application.Dto.Response;

public class FareRuleResponse
{
    public Guid Id { get; set; }
    public string Cabin { get; set; } = default!;
    public string FareBasis { get; set; } = default!;
    public decimal BaseFare { get; set; }
}
