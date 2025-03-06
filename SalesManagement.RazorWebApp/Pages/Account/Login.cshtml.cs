using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using SalesManagement.Repository.Dtos;
using SalesManagement.Service;

namespace SalesManagement.RazorWebApp.Pages.Account;

public class LoginModel : PageModel
{
    private UserAccountService _userAccountService;

    public LoginModel(UserAccountService userAccountService)
    {
        _userAccountService = userAccountService;
    }

    [BindProperty]
    public LoginDto Input { get; set; }


    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            var user = await _userAccountService.LoginAsync(Input.UserName, Input.Password);
            if (user is not null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, Input.UserName),
                    new Claim(ClaimTypes.Role, user.RoleId.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return RedirectToPage("/Index");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }

        return Page();
    }
}