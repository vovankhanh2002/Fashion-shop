using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using BanDoWeb.Areas.Hubs;
using BanDoWeb.Model.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Project.DataAccess.Repository.IRepository;

namespace BanDoWeb.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<SignalsServer> hubContext;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManage,
            IUnitOfWork unitOfWork, IHubContext<SignalsServer> hubContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManage;
            _unitOfWork = unitOfWork;
            this.hubContext = hubContext;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            public string Name { get; set; }
            public string? StreetAddress { get; set; }
            public string? City { get; set; }
            public string? State { get; set; }
            public string? PostalCode { get; set; }
            public string? Role { get; set; }
            public int? Company { get; set; }
            public IEnumerable<SelectListItem> selectListItems { get; set; }
            public IEnumerable<SelectListItem> companyListItems { get; set; }

        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!_roleManager.RoleExistsAsync(SD.Role_User_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Company)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Indi)).GetAwaiter().GetResult();
            }
            Input = new InputModel()
            {
                selectListItems = _roleManager.Roles.Select(n => n.Name).Select(i => 
                    new SelectListItem
                    {
                        Text = i,
                        Value = i

                    }),
                companyListItems = _unitOfWork.Company.GetAll().Select(i =>
                    new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString(),
                    })
            };
            
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState!= null)
            {
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    Name = Input.Name,
                    StreetAddress = Input.StreetAddress,
                    City = Input.City,
                    PostalCode = Input.PostalCode,
                    State = Input.State,
                    Date = DateTime.Now
                };
                if(Input.Role == SD.Role_User_Company)
                {
                    user.CompanyId = Input.Company;
                }
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Bạn đã tạo thành công tài khoảng mới.");
                    if (Input.Role == null)
                    {
                        await _userManager.AddToRoleAsync(user, SD.Role_User_Indi);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, Input.Role);
                    }
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, url = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Bạn cần xác nhận email",
                        $"Làm ơn xác nhận email của bạn <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                    await hubContext.Clients.All.SendAsync("LoadOrderHeader");

                    if (_userManager.Options.SignIn.RequireConfirmedEmail)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        var NumberOfVisit = _unitOfWork.NumberOfVisits.GetById(i => i.ApplicationUserId == user.Id);
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
                            numBerOfVs.ApplicationUserId = user.Id;
                            _unitOfWork.NumberOfVisits.Add(numBerOfVs);
                            _unitOfWork.Save();
                        }
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
