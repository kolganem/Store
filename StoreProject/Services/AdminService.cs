using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoreProject.Models;
using StoreProject.ViewModel;

namespace StoreProject.Services;

[Authorize(Roles ="Admin")]
public class AdminService : IAdminService
{
    private readonly StoreDbContext _context;
    private readonly UserManager<User> _userManager;

    public AdminService(StoreDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IEnumerable<UserRole>> GetListUserWithRolesAsync()
    {
        List<User> users = await _context.Users.ToListAsync();
        List<UserRole> userModels = new List<UserRole>();

        foreach (User user in users)
        {
            IList<string>? userRoles = await _userManager.GetRolesAsync(user);
            userModels.Add(new UserRole { User = user, Roles = userRoles });                
        }

        return userModels;
    }
}
