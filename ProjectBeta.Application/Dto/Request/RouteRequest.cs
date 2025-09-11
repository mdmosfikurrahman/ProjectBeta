namespace ProjectBeta.Application.Dto.Request;

public class RouteRequest
{
    public string Origin { get; set; } = default!;
    public string Destination { get; set; } = default!;
    public int DistanceKm { get; set; }
}
