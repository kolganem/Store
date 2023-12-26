using StoreProject.Models;

namespace StoreProject.Services;

public interface IAdminService
{
    Task<IEnumerable<UserRole>> GetListUserWithRolesAsync();
}