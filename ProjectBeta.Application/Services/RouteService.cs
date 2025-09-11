using System.Collections.Concurrent;
using ProjectBeta.Application.Dto.Request;
using ProjectBeta.Application.Dto.Response;
using ProjectBeta.Domain.Entities;
using ProjectBeta.Shared.Dto;
using static ProjectBeta.Shared.Dto.ApiResponseHelper;

namespace ProjectBeta.Application.Services;

public interface IRouteService
{
    Task<StandardResponse> GetAllAsync();
    Task<StandardResponse> GetByIdAsync(Guid id);
    Task<StandardResponse> CreateAsync(RouteRequest request);
}


public class RouteService : IRouteService
{
    private readonly ConcurrentDictionary<Guid, Route> _store = new();
    private const string EntityName = "Route";

    public RouteService()
    {
        var r1 = new Route { Id = Guid.NewGuid(), Origin = "DAC", Destination = "CXB", DistanceKm = 392 };
        var r2 = new Route { Id = Guid.NewGuid(), Origin = "DAC", Destination = "ZYL", DistanceKm = 199 };
        _store[r1.Id] = r1; _store[r2.Id] = r2;
    }

    public Task<StandardResponse> GetAllAsync()
    {
        var list = _store.Values.Select(ToResponse).ToList();
        return Task.FromResult(list.Count == 0 ? NotFound($"{EntityName}s") : Success($"{EntityName}s", list));
    }

    public Task<StandardResponse> GetByIdAsync(Guid id)
    {
        return Task.FromResult(!_store.TryGetValue(id, out var entity)
            ? NotFound($"{EntityName} with Id {id}")
            : Success(EntityName, ToResponse(entity)));
    }

    public Task<StandardResponse> CreateAsync(RouteRequest request)
    {
        var errors = new List<ErrorDetails>();
        if (string.IsNullOrWhiteSpace(request.Origin)) errors.Add(new ErrorDetails(nameof(request.Origin), "Origin is required."));
        if (string.IsNullOrWhiteSpace(request.Destination)) errors.Add(new ErrorDetails(nameof(request.Destination), "Destination is required."));
        if (request.DistanceKm <= 0) errors.Add(new ErrorDetails(nameof(request.DistanceKm), "DistanceKm must be positive."));

        if (errors.Count > 0) return Task.FromResult(ValidationError(EntityName, errors));

        var entity = new Route
        {
            Id = Guid.NewGuid(),
            Origin = request.Origin.Trim(),
            Destination = request.Destination.Trim(),
            DistanceKm = request.DistanceKm
        };
        _store[entity.Id] = entity;

        return Task.FromResult(Created(EntityName, ToResponse(entity)));
    }

    private static RouteResponse ToResponse(Route r) => new()
    {
        Id = r.Id, Origin = r.Origin!, Destination = r.Destination!, DistanceKm = r.DistanceKm
    };
}
