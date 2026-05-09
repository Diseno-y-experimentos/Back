using BusTrackBackEnd.API.IAM.Domain.Model.Aggregates;
using BusTrackBackEnd.API.IAM.Domain.Model.Queries;
using BusTrackBackEnd.API.IAM.Domain.Repositories;

namespace BusTrackBackEnd.API.IAM.Application.Internal.QueryServices;

public class UserQueryService : IUserQueryService
{
    private readonly IUserRepository _repository;

    public UserQueryService(IUserRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<User>> Handle(GetAllUsersQuery query)
        => _repository.ListAsync();

    public Task<User?> Handle(GetUserByIdQuery query)
        => _repository.FindByIdAsync(query.Id);

    public Task<User?> Handle(GetUserByUsernameQuery query)
        => _repository.FindByUsernameAsync(query.Username);
}