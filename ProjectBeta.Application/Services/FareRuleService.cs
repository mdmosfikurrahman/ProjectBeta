using System.Collections.Concurrent;
using ProjectBeta.Shared.Dto;
using ProjectBeta.Application.Dto.Request;
using ProjectBeta.Application.Dto.Response;
using ProjectBeta.Domain.Entities;
using static ProjectBeta.Shared.Dto.ApiResponseHelper;
namespace ProjectBeta.Application.Services;

public interface IFareRuleService
{
    Task<StandardResponse> GetAllAsync();
    Task<StandardResponse> GetByIdAsync(Guid id);
    Task<StandardResponse> CreateAsync(FareRuleRequest request);
}

public class FareRuleService : IFareRuleService
{
    private readonly ConcurrentDictionary<Guid, FareRule> _store = new();
    private const string EntityName = "FareRule";

    public FareRuleService()
    {
        var f1 = new FareRule { Id = Guid.NewGuid(), Cabin = "Economy", FareBasis = "Y",  BaseFare = 99.99m };
        var f2 = new FareRule { Id = Guid.NewGuid(), Cabin = "Business", FareBasis = "J", BaseFare = 399.00m };
        _store[f1.Id] = f1; _store[f2.Id] = f2;
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

    public Task<StandardResponse> CreateAsync(FareRuleRequest request)
    {
        var errors = new List<ErrorDetails>();
        if (string.IsNullOrWhiteSpace(request.Cabin)) errors.Add(new ErrorDetails(nameof(request.Cabin), "Cabin is required."));
        if (string.IsNullOrWhiteSpace(request.FareBasis)) errors.Add(new ErrorDetails(nameof(request.FareBasis), "FareBasis is required."));
        if (request.BaseFare < 0) errors.Add(new ErrorDetails(nameof(request.BaseFare), "BaseFare cannot be negative."));

        if (errors.Count > 0) return Task.FromResult(ValidationError(EntityName, errors));

        var entity = new FareRule
        {
            Id = Guid.NewGuid(),
            Cabin = request.Cabin.Trim(),
            FareBasis = request.FareBasis.Trim(),
            BaseFare = request.BaseFare
        };
        _store[entity.Id] = entity;

        return Task.FromResult(Created(EntityName, ToResponse(entity)));
    }

    private static FareRuleResponse ToResponse(FareRule f) => new()
    {
        Id = f.Id, Cabin = f.Cabin!, FareBasis = f.FareBasis!, BaseFare = f.BaseFare
    };
}

