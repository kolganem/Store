using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreProject.Models;
using StoreProject.Services;
using StoreProject.ViewModel;

namespace StoreProject.Controllers;

[Authorize(Roles = Roles.Administrator)]
public class AdminController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<AdminController> _logger;
    private readonly IAdminService _adminService;

    public AdminController(UserManager<User> userManager, ILogger<AdminController> logger,
        IAdminService adminService)
    {
        _userManager = userManager;
        _logger = logger;
        _adminService = adminService;
    }
    // GET
    public async Task<IActionResult> Index()
    {
        IEnumerable<UserRole> users = await _adminService.GetListUserWithRolesAsync();
        return View(users);
    }
    
    // GET
    public async Task<IActionResult> ChangePassword(string userId)
    {
        User? user = await _userManager.Users.SingleAsync(u => u.Id == userId);
        
        if (user == null)
        {
            ModelState.AddModelError("UserNotExistProblem", "userNotExist");
        }
       
        ChangePasswordViewModel changePasswordViewModel = new()
        {
            UserId = userId
        };

        return user != null ? View(changePasswordViewModel) : View();
    }
    
    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
    {
        if (ModelState.IsValid)
        {
            User user = await _userManager.FindByIdAsync(changePasswordViewModel.UserId);
            string? token = await _userManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult? identityResult = await _userManager.ResetPasswordAsync(user, token, 
                changePasswordViewModel.NewPassword);

            if (identityResult.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            string? errorMessage = identityResult.Errors.Select(e => $"{e.Code} -- ${e.Description}").ToString();

            _logger.LogError(errorMessage);
            ModelState.AddModelError("ChangePasswordProblem", "Can't change password for user");
        }

        return View();
    }
    
    public IActionResult CreateUser()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserViewModel createUserViewModel)
    {
        if (ModelState.IsValid)
        {
            if (_userManager.Users.Any(u => u.Email == createUserViewModel.Email))
            {
                ModelState.AddModelError("DuplicateEmails", "User with email like this already exist");
            }
            else
            {
                User newUser = new User
                {
                    Email = createUserViewModel.Email,
                    EmailConfirmed = true,
                    UserName = createUserViewModel.Email
                };
                newUser.FirstName = newUser.Email.ToUpper();
                newUser.NormalizedEmail = newUser.Email.ToUpper();
                IdentityResult? createResult = await _userManager.CreateAsync(newUser, createUserViewModel.Password);
                if (createResult.Succeeded)
                {
                    CustomUserRole role = CustomUserRole.User;
                    var addRoleResult = await _userManager.AddToRoleAsync(newUser, role.ToString());

                    if (addRoleResult.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    string? errorMessage = addRoleResult.Errors.Select(e => $"{e.Code} -- ${e.Description}").ToString();

                    _logger.LogError(errorMessage);
                    ModelState.AddModelError("AddToRoleProblem", "Can't add role to user");
                }
                else
                {
                    ModelState.AddModelError("CreateUserProblem", "Can't create user");
                }
            }
        }

        return View();
    }
    
    [AllowAnonymous]
    public IActionResult CreateAdmin()
    {
        return View();
    }
    
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> CreateAdmin(CreateUserViewModel createUserViewModel)
    {
        if (ModelState.IsValid)
        {
            if (_userManager.Users.Any(u => u.Email == createUserViewModel.Email))
            {
                ModelState.AddModelError("DuplicateEmails", "User with email like this already exist");
            }
            else
            {
                User newUser = new User
                {
                    Email = createUserViewModel.Email,
                    EmailConfirmed = true,
                    UserName = createUserViewModel.Email
                };
                newUser.FirstName = newUser.Email.ToUpper();
                newUser.NormalizedEmail = newUser.Email.ToUpper();
                IdentityResult? createResult = await _userManager.CreateAsync(newUser, createUserViewModel.Password);
                if (createResult.Succeeded)
                {
                    CustomUserRole role = CustomUserRole.Administrator;
                    var addRoleResult = await _userManager.AddToRoleAsync(newUser, role.ToString());

                    if (addRoleResult.Succeeded)
                    {
                        return RedirectToAction(nameof(Index), "Home");
                    }

                    string? errorMessage = addRoleResult.Errors.Select(e => $"{e.Code} -- ${e.Description}").ToString();

                    _logger.LogError(errorMessage);
                    ModelState.AddModelError("AddToRoleProblem", "Can't add role to user");
                }
                else
                {
                    ModelState.AddModelError("CreateUserProblem", "Can't create user");
                }
            }
        }

        return View();
    }
    
    [HttpGet]
    public async Task<IActionResult> Delete(string userId)
    {
        User? user = await _userManager.FindByIdAsync(userId);

        if (user != null)
        {
            return View(user);
        }

        return BadRequest();
    }
    
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(string userId)
    {
        User? user = await _userManager.FindByIdAsync(userId);
        var deleteResult = await _userManager.DeleteAsync(user);

        if (deleteResult.Succeeded)
        {
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError("DeleteUserProblem", "Can't delete user");

        return View();
    }
    
    [HttpGet]
    public async Task<IActionResult> Lock(string userId)
    {
        User? user = await _userManager.FindByIdAsync(userId);

        if (user != null)
        {
            return View(user);
        }

        return BadRequest();
    }
    
    [HttpPost, ActionName("Lock")]
    public async Task<IActionResult> LockConfirmed(string userId)
    {
        User? user = await _userManager.FindByIdAsync(userId);
        DateTime lockOutDate = DateTime.Now.AddDays(1);
        var lockoutResult = await _userManager.SetLockoutEndDateAsync(user, lockOutDate);

        if (lockoutResult.Succeeded)
        {
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError("LockOutProblem", "Can't lock user to one day");

        return View();
    }
}