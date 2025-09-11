namespace ProjectBeta.Application.Dto.Response;

public class RouteResponse
{
    public Guid Id { get; set; }
    public string Origin { get; set; } = default!;
    public string Destination { get; set; } = default!;
    public int DistanceKm { get; set; }
}
