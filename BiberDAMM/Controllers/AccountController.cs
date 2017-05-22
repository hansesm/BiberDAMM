using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using BiberDAMM.Models;
using BiberDAMM.Security;
using BiberDAMM.Helpers;
using BiberDAMM.DAL;
using System.Collections.Generic;

namespace BiberDAMM.Controllers
{
    [CustomAuthorize]
    public class AccountController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // TODO [KrabsJ] make search caseinsensitive & maybe think about a better solution than always access the db --> wait until Michi and Leon checked if we use other searchfunctions
        // if we need a custom caseinsensitive search follow this link: http://stackoverflow.com/questions/444798/case-insensitive-containsstring
        // GET: /Account/Index
        // returns a list with all ApplicationUsers that match the searchString [KrabsJ]
        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator)]
        public ActionResult Index(string searchString)
        {
            var ApplicationUsers = db.Users.ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                var filterResult = ApplicationUsers.Where(a => a.UserName.Contains(searchString) || a.Surname.Contains(searchString) || a.Lastname.Contains(searchString) || a.UserType.ToString().Contains(searchString));
                return View(filterResult.OrderBy(a => a.UserName));
            }

            return View(ApplicationUsers.OrderBy(a => a.UserName));
        }

        // GET: /Account/Details
        // returns the details of a single ApplicationUser [KrabsJ]
        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator)]
        public ActionResult Details(int? userId)
        {
            if (userId == null)
            {
                return RedirectToAction("Index");
            }
            int id = userId ?? default(int);
            ApplicationUser user = UserManager.FindById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // TODO [KrabsJ] add edit method and view
        // TODO [KrabsJ] add details method and view
        // TODO [KrabsJ] add delete method and view

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Anmeldefehler werden bezüglich einer Kontosperre nicht gezählt.
            // Wenn Sie aktivieren möchten, dass Kennwortfehler eine Sperre auslösen, ändern Sie in "shouldLockout: true".
            // section changed: login should be based on Username instead of Email and there should be no rememberMe function so it is set to "false" [KrabsJ]
            // var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);

            ApplicationUser requestingUser = UserManager.FindByName(model.Username);
            if(requestingUser != null && requestingUser.Active == false)
            {
                //Added errormessage name to dispay in loginview [HansesM]
                ModelState.AddModelError("InactiveUser", "Der angegebene Nutzer ist inaktiv. Bitte wenden Sie sich an einen Administrator.");
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(model.Username, model.Password, isPersistent: false, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    // if there is a returnUrl (this happens if a user who is not logged in tries to call a method), the application should redirect to the required page after login [KrabsJ]
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    };
                case SignInStatus.LockedOut:
                    return View("Lockout");
                // section changed: there should be no rememberMe function --> so it is set to "false" [KrabsJ]
                //case SignInStatus.RequiresVerification:
                    //return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });

                //Added errormessage name to dispay in loginview [HansesM]
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("WrongUsernameOrPass", "Falscher Benutzername oder falsches Passwort");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Verlangen, dass sich der Benutzer bereits mit seinem Benutzernamen/Kennwort oder einer externen Anmeldung angemeldet hat.
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Der folgende Code schützt vor Brute-Force-Angriffen der zweistufigen Codes. 
            // Wenn ein Benutzer in einem angegebenen Zeitraum falsche Codes eingibt, wird das Benutzerkonto 
            // für einen bestimmten Zeitraum gesperrt. 
            // Sie können die Einstellungen für Kontosperren in "IdentityConfig" konfigurieren.
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Ungültiger Code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        // changed line: register only available for administrator [KrabsJ]
        //[AllowAnonymous]
        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator)]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        // changed line: register only available for administrator [KrabsJ]
        //[AllowAnonymous]
        [HttpPost]
        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // section added: variables for the algorithm of creating the Username [KrabsJ]
                string username = model.Lastname + model.Surname[0];
                string usernameWithNumber;
                ApplicationUser userdb;
                int surnameCounter = model.Surname.Length;
                int userNameNumber = 1;

                //section added: algorithm of creating the Username [KrabsJ]
                //Username should be the Lastname plus the first character of the Surname
                //If there is already another user with the same name the next character of the surname will be added and so on
                //If there is already another user with the same name including the whole lastname plus surname a sequential number will be added
                for (int i = 0; i < surnameCounter; i++)
                {
                    userdb = UserManager.FindByName(username);
                    if (userdb == null)
                    {
                        break;
                    }
                    else
                    {
                        if ((i + 1) < surnameCounter)
                        {
                            username = username + model.Surname[i + 1];
                        }
                        else
                        {
                            usernameWithNumber = username + userNameNumber.ToString();
                            while (true)
                            {
                                userdb = UserManager.FindByName(usernameWithNumber);
                                if (userdb == null)
                                {
                                    username = usernameWithNumber;
                                    break;
                                }
                                else
                                {
                                    userNameNumber++;
                                    usernameWithNumber = username + userNameNumber.ToString();
                                }
                            }
                        }

                    }
                }

                //section changed: depending on the expansion of the ApplicationUser class there are more attributes that are necessary to create a new user
                //var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var user = new ApplicationUser { UserName = username, Email = model.Email, Surname = model.Surname, Lastname = model.Lastname, Active=model.Active, UserType=model.UserType, PhoneNumber=model.PhoneNumber };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // section deleted: the register-method is only available for the administrator, there is no need to login the new user
                    // await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    //based on the usertype the new user will get a role to give specific access to functionalities [KrabsJ]
                    switch (user.UserType)
                    {
                        case UserType.Reinigungskraft:
                            UserManager.AddToRole(user.Id, ConstVariables.RoleCleaner);
                            break;
                        case UserType.Pflegekraft:
                            UserManager.AddToRole(user.Id, ConstVariables.RoleNurse);
                            break;
                        case UserType.Arzt:
                            UserManager.AddToRole(user.Id, ConstVariables.RoleDoctor);
                            break;
                        case UserType.Administrator:
                            UserManager.AddToRole(user.Id, ConstVariables.RoleAdministrator);
                            break;
                        case UserType.Therapeut:
                            UserManager.AddToRole(user.Id, ConstVariables.RoleTherapist);
                            break;
                        default:
                            //TODO [KrabsJ] throw custom exception
                            break;
                    }

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // E-Mail-Nachricht mit diesem Link senden
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Konto bestätigen", "Bitte bestätigen Sie Ihr Konto. Klicken Sie dazu <a href=\"" + callbackUrl + "\">hier</a>");

                    // success-message for alert-statement [KrabsJ]
                    TempData["CreateUserSuccess"] = " Benutzername: " + user.UserName;

                    // Redirect to list of all users [KrabsJ]
                    return RedirectToAction("Index", "Account");
                }
                AddErrors(result);
            }

            // Wurde dieser Punkt erreicht, ist ein Fehler aufgetreten; Formular erneut anzeigen.
            // failed-message for alert-statement [KrabsJ]
            TempData["CreateUserFailed"] = " Bei der Registrierung ist ein Fehler aufgetreten";
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        // Change PrimaryKey of identity package to int [public async Task<ActionResult> ConfirmEmail(string userId, string code)] //KrabsJ
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(int userId, string code)
        {
            //Change PrimaryKey of identity package to int [if (userId == null || code == null)] //KrabsJ
            if (userId == default(int) || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Nicht anzeigen, dass der Benutzer nicht vorhanden ist oder nicht bestätigt wurde.
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // E-Mail-Nachricht mit diesem Link senden
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Kennwort zurücksetzen", "Bitte setzen Sie Ihr Kennwort zurück. Klicken Sie dazu <a href=\"" + callbackUrl + "\">hier</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // Wurde dieser Punkt erreicht, ist ein Fehler aufgetreten; Formular erneut anzeigen.
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Nicht anzeigen, dass der Benutzer nicht vorhanden ist.
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Umleitung an den externen Anmeldeanbieter anfordern
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            // Change PrimaryKey of identity package to int [if (userId == null)] //KrabsJ
            if (userId == default(int))
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Token generieren und senden
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Benutzer mit diesem externen Anmeldeanbieter anmelden, wenn der Benutzer bereits eine Anmeldung besitzt
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // Benutzer auffordern, ein Konto zu erstellen, wenn er kein Konto besitzt
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Informationen zum Benutzer aus dem externen Anmeldeanbieter abrufen
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }

                // dispose ApplicationDbContext instance [KrabsJ]
                db.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Hilfsprogramme
        // Wird für XSRF-Schutz beim Hinzufügen externer Anmeldungen verwendet
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}