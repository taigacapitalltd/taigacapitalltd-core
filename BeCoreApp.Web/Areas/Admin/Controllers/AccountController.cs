using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using BeCoreApp.Models;
using BeCoreApp.Models.AccountViewModels;
using BeCoreApp.Services;
using BeCoreApp.Data.Entities;
using BeCoreApp.Data.Enums;
using PaulMiami.AspNetCore.Mvc.Recaptcha;
using BeCoreApp.Areas.Admin.Controllers;
using BeCoreApp.Utilities.Dtos;
using BeCoreApp.Extensions;
using BeCoreApp.Application.Interfaces;
using System.Text.Encodings.Web;

namespace BeCoreApp.Areas.Admin.Controllers
{
    public class AccountController : BaseController
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserService _userService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IBlockChainService _blockChainService;
        private readonly ILogger _logger;
        private readonly IViewRenderService _viewRenderService;

        public AccountController(
            IBlockChainService blockChainService,
            IUserService userService,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IEmailSender emailSender,
            RoleManager<AppRole> roleManager,
            IViewRenderService viewRenderService,
            ILogger<AccountController> logger)
        {
            _blockChainService = blockChainService;
            _userService = userService;
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _viewRenderService = viewRenderService;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");

            return RedirectToAction("Login", "/Admin/Account/");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            await _signInManager.SignOutAsync();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginVm)
        {

            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.FindByNameAsync(loginVm.UserName);

                if (currentUser == null)
                    return new OkObjectResult(new GenericResult(false, "Đăng nhập sai"));

                if (currentUser.EmailConfirmed == false)
                    return new OkObjectResult(new GenericResult(false, "Email chưa được xác nhận"));

                if (currentUser.Status != Status.Active)
                    return new OkObjectResult(new GenericResult(false, "Tài khoản đã bị khóa"));

                var result = await _signInManager.PasswordSignInAsync(loginVm.UserName, loginVm.Password, loginVm.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Đăng nhập thành công.");
                    return new OkObjectResult(new GenericResult(true));
                }

                if (result.IsLockedOut)
                {
                    _logger.LogWarning("Tài khoản đã bị khóa.");
                    return new OkObjectResult(new GenericResult(false, "Tài khoản đã bị khóa"));
                }
                else
                {
                    return new OkObjectResult(new GenericResult(false, "Đăng nhập sai."));
                }
            }

            return new OkObjectResult(new GenericResult(false, "Vui lòng nhập đầy đủ thông tin"));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWith2fa(bool rememberMe, string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            var model = new LoginWith2faViewModel { RememberMe = rememberMe };
            ViewData["ReturnUrl"] = returnUrl;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWith2fa(LoginWith2faViewModel model, bool rememberMe, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, model.RememberMachine);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID {UserId} logged in with 2fa.", user.Id);
                return RedirectToLocal(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                _logger.LogWarning("Invalid authenticator code entered for user with ID {UserId}.", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithRecoveryCode(string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            var recoveryCode = model.RecoveryCode.Replace(" ", string.Empty);

            var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID {UserId} logged in with a recovery code.", user.Id);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                _logger.LogWarning("Invalid recovery code entered for user with ID {UserId}", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid recovery code entered.");
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ActiveCustomer(string transactionID)
        //{
        //    try
        //    {
        //        var userId = User.GetSpecificClaim("UserId");
        //        var result = await _blockChainService.CheckActiveAccountByTransactionID(transactionID, userId);
        //        if (result.Success)
        //        {
        //            //await _userService.UpdateCustomerActivated("");
        //        }

        //        return new OkObjectResult(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new OkObjectResult(new GenericResult(false, ex.Message));
        //    }
        //}

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);

            return RedirectToAction("Login", "/Admin/Account/");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string refId)
        {
            var model = new RegisterViewModel()
            {
                SponsorNo = refId
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        //[ValidateRecaptcha]
        public async Task<IActionResult> Register(RegisterViewModel registerVm)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new OkObjectResult(new GenericResult(false, string.Join(',', ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage))));

                if (registerVm.SponsorNo.Length < 6)
                    return new OkObjectResult(new GenericResult(false, "Nhà tài trợ không tồn tại!!!"));

                var sponsorString = registerVm.SponsorNo.Substring(2);

                bool isValidSponsor = int.TryParse(sponsorString, out int sponsorNo);
                if (!isValidSponsor)
                    return new OkObjectResult(new GenericResult(false, "Nhà tài trợ không tồn tại!!!"));

                var userSponsor = _userManager.Users.FirstOrDefault(x => x.SponsorNo == sponsorNo);
                if (userSponsor == null)
                    return new OkObjectResult(new GenericResult(false, "Nhà tài trợ không tồn tại!!!"));

                var currentUserEmail = await _userManager.FindByEmailAsync(registerVm.Email);
                if (currentUserEmail != null)
                    return new OkObjectResult(new GenericResult(false, "Email đã tồn tại!!!"));

                var currentUserCMND = _userManager.Users.FirstOrDefault(x => x.CMND.ToLower() == registerVm.CMND.ToLower());
                if (currentUserCMND != null)
                    return new OkObjectResult(new GenericResult(false, "CMND đã tồn tại!!!"));

                var currentUserPhoneNumber = _userManager.Users.FirstOrDefault(x => x.PhoneNumber.ToLower() == registerVm.PhoneNumber.ToLower());
                if (currentUserPhoneNumber != null)
                    return new OkObjectResult(new GenericResult(false, "Số điện thoại đã tồn tại!!!"));

                var currentUserName = await _userManager.FindByNameAsync(registerVm.UserName);
                if (currentUserName != null)
                    return new OkObjectResult(new GenericResult(false, "Tài khoản đã tồn tại!!!"));

                var ethereumAccount = _blockChainService.CreateEthereumAccount();

                int newSponsorNo = _userManager.Users.Max(x => x.SponsorNo ?? 0);

                var customer = new AppUser
                {
                    UserName = registerVm.UserName,
                    FullName = registerVm.FullName,
                    CMND = registerVm.CMND,
                    SponsorNo = newSponsorNo + 1,
                    PhoneNumber = registerVm.PhoneNumber,
                    PublishKey = ethereumAccount.Address,
                    PrivateKey = ethereumAccount.PrivateKey,
                    Email = registerVm.Email,
                    ReferralId = userSponsor.Id,
                    WalletStart = 0,
                    WalletGame = 0,
                    WalletWin = 0,
                    WalletValuesShare = 0,
                    WalletUsdt = 0,
                    WalletEthereum = 0,
                    WalletUsdtReferral = 0,
                    IsSystem = false,
                    IsUpdatedKYC = false,
                    UpdateKYCDate = null,
                    IsActivated = false,
                    ActivatedDate = null,
                    Status = Status.Active,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now
                };

                var result = await _userManager.CreateAsync(customer, registerVm.Password);
                if (result.Succeeded)
                {
                    var appRole = await _roleManager.FindByNameAsync("Customer");
                    if (appRole == null)
                    {
                        await _roleManager.CreateAsync(new AppRole
                        {
                            Name = "Customer",
                            NormalizedName = "Customer",
                            Description = "Customer is role use for member"
                        });
                    }
                    await _userManager.AddToRoleAsync(customer, "Customer");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(customer);
                    var callbackUrl = Url.EmailConfirmationLink(customer.Id, code, Request.Scheme);

                    var content = await _viewRenderService.RenderToStringAsync("EmailTemplate/_VerifyAccount", callbackUrl);

                    await _emailSender.SendEmailAsync(registerVm.Email, "Salanests.Com: Xác nhận email", content);

                    return new OkObjectResult(new GenericResult(true, "Tài khoản đã đăng ký thành công, Vui lòng kiểm tra email để xác nhận tài khoản"));
                }
                else
                {
                    return new OkObjectResult(new GenericResult(false, string.Join(',', result.Errors.Select(x => x.Description))));
                }
            }
            catch (Exception ex)
            {
                return new OkObjectResult(new GenericResult(false, ex.Message));
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterConfirm()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new OkObjectResult(new GenericResult(false, string.Join(',', ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage))));
                }

                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return new OkObjectResult(new GenericResult(false, "Địa chỉ email của bạn email không tồn tại."));
                }

                if (!(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    return new OkObjectResult(new GenericResult(false, "Địa chỉ email của bạn chưa được xác minh."));
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);

                var content = await _viewRenderService.RenderToStringAsync("EmailTemplate/_ForgotPassword", callbackUrl);

                await _emailSender.SendEmailAsync(model.Email, "Salanests.Com: Đặt lại password", content);

                return new OkObjectResult(new GenericResult(true));
            }
            catch (Exception ex)
            {
                return new OkObjectResult(new GenericResult(false, ex.Message));
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation() => View();


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new OkObjectResult(new GenericResult(false, string.Join(',', ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage))));
                }

                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return new OkObjectResult(new GenericResult(false, "Your email does not exists"));
                }

                var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
                if (result.Succeeded)
                {
                    return new OkObjectResult(new GenericResult(true));
                }
                else
                {
                    return new OkObjectResult(new GenericResult(false, string.Join(',', result.Errors.Select(x => x.Description))));
                }
            }
            catch (Exception ex)
            {
                return new OkObjectResult(new GenericResult(false, ex.Message));
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToAction(nameof(Login));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLogin", new ExternalLoginViewModel());
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                var user = new AppUser
                {
                    UserName = email,
                    Email = email,
                    FullName = model.FullName,
                    BirthDay = DateTime.Parse(model.DOB),
                    PhoneNumber = model.PhoneNumber
                };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(nameof(ExternalLogin), model);
        }



        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}
