namespace ProjectBeta.Application.Dto.Request;

public class FareRuleRequest
{
    public string Cabin { get; set; } = default!;
    public string FareBasis { get; set; } = default!;
    public decimal BaseFare { get; set; }
}
