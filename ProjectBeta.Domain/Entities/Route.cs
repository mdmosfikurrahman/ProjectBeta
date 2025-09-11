namespace ProjectBeta.Domain.Entities;

public class Route
{
    public Guid Id { get; set; }
    public string? Origin { get; set; }
    public string? Destination { get; set; }
    public int DistanceKm { get; set; }
}
