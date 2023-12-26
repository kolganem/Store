using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StoreProject.Models;
using StoreProject.ViewModel;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace StoreProject.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, 
        ILogger<AccountController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }
    
    [HttpGet]
    public IActionResult LogIn()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> LogIn(LoginViewModel loginViewModel)
    {
        if (ModelState.IsValid)
        {
            if (_userManager.Users.Any(u => u.Email == loginViewModel.Email))
            {
                
                SignInResult? signInResult = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, true, lockoutOnFailure: true);
                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                _logger.LogError($"Invalid login attempt for userEmail - {loginViewModel.Email}");
                ModelState.AddModelError("SignInFailure", "SignIn problem");

            }
            
            ModelState.AddModelError("UserNotExist", "User not exist");
        }
        else
        {
            ModelState.AddModelError("ModelValidationError", "Check entered data");
        }

        return View();
    }
    
    [HttpGet]
    public  IActionResult LogOut(string returnUrl)
    {
        return Redirect(returnUrl);
    }
    
    [HttpPost, ActionName("LogOut")]
    public async Task<IActionResult> LogOutConfirmed()
    {
        if (_signInManager.IsSignedIn(User))
        {
            await _signInManager.SignOutAsync();
            return View();
        }

        return RedirectToAction("Index", "Home");
    }
    
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
        if (ModelState.IsValid)
        {
            if (_userManager.Users.Any(u => u.Email == registerViewModel.Email))
            {
                ModelState.AddModelError("DuplicateEmails", "User with email like this already exist");
            }
            else
            {
                User newUser = new User
                {
                    Email = registerViewModel.Email,
                    EmailConfirmed = true,
                    UserName = registerViewModel.Email
                };
                newUser.FirstName = newUser.Email.ToUpper();
                newUser.NormalizedEmail = newUser.Email.ToUpper();
                var createResult = await _userManager.CreateAsync(newUser, registerViewModel.Password);
                if (createResult.Succeeded)
                {
                    CustomUserRole role = (CustomUserRole)registerViewModel.EnteredRole;
                    var addRoleResult = await _userManager.AddToRoleAsync(newUser, role.ToString());

                    if (addRoleResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(newUser, true);
                        return RedirectToAction("Index", "Home");
                    }

                    string? errorMessage = addRoleResult.Errors.Select(e => $"{e.Code} -- ${e.Description}").ToString();

                    _logger.LogError(errorMessage);
                    ModelState.AddModelError("AddToRoleProblem", "Can't add role to user");
                }
                else
                {
                    string? errorMessage = createResult.Errors.Select(e => $"{e.Code} -- ${e.Description}").ToString();

                    _logger.LogError(errorMessage);
                    ModelState.AddModelError("CreateUserProblem", "Can't create user");
                }
            }
        }

        return View();
    }

}