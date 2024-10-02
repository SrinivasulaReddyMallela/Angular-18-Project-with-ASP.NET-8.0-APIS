using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using IdentityStandaloneMfa.Data;
using IdentityStandaloneMfa.SSO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IdentityStandaloneMfa.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class LoginWith2faModel : PageModel
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ILogger<LoginWith2faModel> _logger;
    private readonly IConfiguration _configuration;
    private readonly DatabaseContext _context;
    public LoginWith2faModel(SignInManager<IdentityUser> signInManager, ILogger<LoginWith2faModel> logger, DatabaseContext context, IConfiguration configuration)
    {
        _signInManager = signInManager;
        _logger = logger;
        _context = context;
        _configuration = configuration;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public bool RememberMe { get; set; }

    public string ReturnUrl { get; set; }

    public class InputModel
    {
        [Required]
        [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Authenticator code")]
        public string TwoFactorCode { get; set; }

        [Display(Name = "Remember this machine")]
        public bool RememberMachine { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(bool rememberMe, string returnUrl = null)
    {
        // Ensure the user has gone through the username & password screen first
        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

        if (user == null)
        {
            throw new InvalidOperationException($"Unable to load two-factor authentication user.");
        }

        ReturnUrl = returnUrl;
        RememberMe = rememberMe;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(bool rememberMe, string returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        returnUrl = returnUrl ?? Url.Content("~/");

        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        if (user == null)
        {
            throw new InvalidOperationException($"Unable to load two-factor authentication user.");
        }

        var authenticatorCode = Input.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

        var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, Input.RememberMachine);

        if (result.Succeeded)
        {

            _logger.LogInformation("User with ID '{UserId}' logged in with 2fa.", user.Id);
            var AppKey = HttpContext.Session.GetString("AppKey");
            var ConfigurationList = (from Configuration in _context.SAMLConfiguration
                                     where Configuration.APPKey == AppKey
                                     select Configuration
                                                    ).ToList();


            var AttributesList = (from Attributes in _context.SAMLAttributes
                                  where Attributes.SAMLConfigurationID == ConfigurationList.ToList().Select(a => a.SAMLConfigurationID).FirstOrDefault()
                                  select Attributes
                                    ).ToList();
            Dictionary<string, string> attrs = new Dictionary<string, string>();
            foreach (var Attribute in AttributesList)
            {
                attrs.Add(Attribute.AttributeName, user.Email.ToString());
            }
            // Set SAML Response
            var samlresult =
                SamlHelper.GetPostSamlResponse(
               ConfigurationList.ToList().Select(a => a.Recipient).FirstOrDefault(),
                ConfigurationList.ToList().Select(a => a.Issuer).FirstOrDefault(),
                ConfigurationList.ToList().Select(a => a.Domain).FirstOrDefault(),
                ConfigurationList.ToList().Select(a => a.Subject).FirstOrDefault(),
               ConfigurationList.ToList().Select(a => a.CertStoreLocation).FirstOrDefault() == "LocalMachine" ? StoreLocation.LocalMachine : StoreLocation.CurrentUser,
               StoreName.Root, X509FindType.FindBySubjectName, null, null,
               ConfigurationList.ToList().Select(a => a.CertFriendlyName).FirstOrDefault(), attrs);

            return Redirect(ConfigurationList.ToList().Select(a => a.Target).FirstOrDefault() + "?SAMLResponse=" + samlresult);
        }
        else if (result.IsLockedOut)
        {
            _logger.LogWarning("User with ID '{UserId}' account locked out.", user.Id);
            return RedirectToPage("./Lockout");
        }
        else
        {
            _logger.LogWarning("Invalid authenticator code entered for user with ID '{UserId}'.", user.Id);
            ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
            return Page();
        }
    }
}