using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using IdentityStandaloneMfa.Common;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using IdentityStandaloneMfa.SSO;
using Microsoft.EntityFrameworkCore;
using IdentityStandaloneMfa.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace IdentityStandaloneMfa.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class LoginModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ILogger<LoginModel> _logger;
    private readonly IConfiguration _configuration;
    private readonly DatabaseContext _context;
    public LoginModel(SignInManager<IdentityUser> signInManager,
        ILogger<LoginModel> logger,
        UserManager<IdentityUser> userManager, DatabaseContext context, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _context = context;
        _configuration = configuration;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public IList<AuthenticationScheme> ExternalLogins { get; set; }

    public string ReturnUrl { get; set; }

    [TempData]
    public string ErrorMessage { get; set; }

    public class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public async Task OnGetAsync(string returnUrl = null)
    {
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }
        if (HttpContext.Request.Query.ContainsKey("AppKey"))
            HttpContext.Session.SetString("AppKey", HttpContext.Request.Query["AppKey"]);
        returnUrl = returnUrl ?? Url.Content("~/");

        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl = returnUrl ?? Url.Content("~/");

        if (ModelState.IsValid)
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
            var AppKey = HttpContext.Session.GetString("AppKey");
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                // Set Attrs
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
                    attrs.Add(Attribute.AttributeName, Input.Email.ToString());
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

        //        var targetUrl = ConfigurationList.ToList().Select(a => a.Target).FirstOrDefault();
        //        var serializedData = JsonConvert.SerializeObject(Encoding.UTF8.GetString(Convert.FromBase64String(samlresult)));

        //        var html = $@"
        //<html>
        //<head>
        //    <script type='text/javascript'>
        //        window.onload = function() {{
        //            var newWindow = window.open('{targetUrl}', '_blank');
        //            if (newWindow) {{
        //                newWindow.onload = function() {{
        //                    var event = new CustomEvent('receiveData', {{ detail: {serializedData} }});
        //                    newWindow.dispatchEvent(event);
        //                }};
        //            }}
        //            window.location.href = '{targetUrl}';
        //        }};
        //    </script>
        //</head>
        //<body>
        //</body>
        //</html>";

        //        return Content(html, "text/html");
                return RedirectPermanent(ConfigurationList.ToList().Select(a => a.Target).FirstOrDefault() + "?SAMLResponse=" + samlresult);
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                return RedirectToPage("./Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }
}
