using datttwebapi.Data;

namespace datttwebapi.Services
{
    public interface IUserService
    {
        Task<IReadOnlyList<User>> GetAll();

    }
}
