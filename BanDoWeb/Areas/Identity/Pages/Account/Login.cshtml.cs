using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using AspNetCoreHero.ToastNotification.Abstractions;
using NToastNotify;
using Project.DataAccess.Repository.IRepository;
using BanDoWeb.Model.Models;
using Microsoft.AspNetCore.SignalR;
using BanDoWeb.Areas.Hubs;

namespace BanDoWeb.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly INotyfService _notyfService;
        private readonly IToastNotification _toastNotification;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<SignalsServer> _hubContext;

        public LoginModel(SignInManager<IdentityUser> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<IdentityUser> userManager,
            INotyfService notyfService,
            IToastNotification toastNotification,
            IHubContext<SignalsServer> hubContext,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _notyfService = notyfService;
            _toastNotification = toastNotification;
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string? ReturnUrl { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string? Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string? Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string? returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        
            if (ModelState!=null)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                var appUser = await _userManager.FindByEmailAsync(Input.Email);
                bool emailStatus = await _userManager.IsEmailConfirmedAsync(appUser);
                if (emailStatus == false )
                {
                    ModelState.AddModelError(string.Empty, "Email chưa xác nhận, làm ơn xác nhận email trước.");
                }
                else if(emailStatus == true )
                {
                    if (result.Succeeded)
                    {
                        var NumberOfVisit = _unitOfWork.NumberOfVisits.GetById(i => i.ApplicationUserId == appUser.Id);
                        if (NumberOfVisit != null)
                        {
                            NumberOfVisit.accessNumber += 1;
                            NumberOfVisit.DateTime = DateTime.Now;
                            _unitOfWork.NumberOfVisits.UpdateNumberOfVisits(NumberOfVisit);
                            _unitOfWork.Save();
                        }
                        else
                        {
                            var numBerOfVs = new NumberOfVisits();
                            numBerOfVs.DateTime = DateTime.Now;
                            numBerOfVs.accessNumber = 1;
                            numBerOfVs.ApplicationUserId = appUser.Id;
                            _unitOfWork.NumberOfVisits.Add(numBerOfVs);
                            _unitOfWork.Save();
                        }
                        await _hubContext.Clients.All.SendAsync("LoadStatis");
                        _toastNotification.AddSuccessToastMessage("Chào mừng bạn đến admin");
                        TempData["toastLogin"] = true;
                        return LocalRedirect(returnUrl);
                    }
                    if (result.RequiresTwoFactor)
                    {
                        TempData["toast"] = true;
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
                
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
