using BusTrackBackEnd.API.IAM.Domain.Model.Queries;

namespace BusTrackBackEnd.API.IAM.Domain.Model.Aggregates;

public interface IUserQueryService
{
    Task<IEnumerable<User>> Handle(GetAllUsersQuery query);
    Task<User?> Handle(GetUserByIdQuery query);
    Task<User?> Handle(GetUserByUsernameQuery query);
}