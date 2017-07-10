using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BiberDAMM.DAL;
using BiberDAMM.Helpers;
using BiberDAMM.Models;
using BiberDAMM.Security;
using BiberDAMM.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace BiberDAMM.Controllers
{
    //[KrabsJ]
    [CustomAuthorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        // GET: /Account/Index
        // returns a list with all ApplicationUsers that match the searchString [KrabsJ]
        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator)]
        public ActionResult Index()
        {
            var ApplicationUsers = db.Users.ToList();
  
            return View(ApplicationUsers);
        }

        // GET: /Account/Details
        // returns the details of a single ApplicationUser [KrabsJ]
        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator)]
        public ActionResult Details(int? userId)
        {
            if (userId == null)
                return RedirectToAction("Index");
            var id = userId ?? default(int);
            var user = UserManager.FindById(id);
            if (user == null)
                return HttpNotFound();
            return View(user);
        }

        // GET: /Acount/Edit
        // returns the edit View for a single ApplicationUser [KrabsJ]
        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator)]
        public ActionResult Edit(int? userId)
        {
            if (userId == null)
                return RedirectToAction("Index");

            var id = userId ?? default(int);
            var user = UserManager.FindById(id);
            if (user == null)
                return HttpNotFound();

            //fill the necessary attributes of editViewModel
            var editUser = new EditViewModel
            {
                Id = user.Id,
                Title = user.Title,
                Surname = user.Surname,
                Lastname = user.Lastname,
                UserType = user.UserType,
                Active = user.Active,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };
            return View(editUser);
        }

        // POST: Account/Edit
        // method stores the changed data of ApplicationUser [KrabsJ]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator)]
        public ActionResult Edit(EditViewModel model, string command)
        {
            if (command.Equals(ConstVariables.AbortButton))
                return RedirectToAction("Details", "Account", new {userId = model.Id});

            if (ModelState.IsValid)
            {
                var changeRole = false;
                var editUser = UserManager.FindById(model.Id);

                if (editUser.Surname != model.Surname || editUser.Lastname != model.Lastname)
                    editUser.UserName = CreateUserName(model.Surname, model.Lastname);

                // check if the userRole has to be changed
                if (editUser.UserType != model.UserType)
                {
                    // check if there are dependencies
                    Stay dependentStay = db.Stays.Where(s => s.ApplicationUserId == model.Id).FirstOrDefault();
                    Treatment dependentTreatment = db.Users.Where(u => u.Id == model.Id).SelectMany(u => u.Treatments).FirstOrDefault();
                    if (dependentStay != null || dependentTreatment != null)
                    {
                        // error-message for alert-statement
                        TempData["ErrorDependenciesOnRole"] = " Der Benutzer ist in seiner Rolle als " + editUser.UserType.ToString() + " noch Behandlungen oder Aufenthalten zugeteilt. Bitte löschen Sie zunächst diese Abhängigkeiten oder legen Sie einen neuen Benutzer an.";
                        return View("Edit", model);
                    }
                    changeRole = true;
                }
                    

                // get the new data from the EditViewModel
                editUser.Title = model.Title;
                editUser.Surname = model.Surname;
                editUser.Lastname = model.Lastname;
                editUser.Email = model.Email;
                editUser.PhoneNumber = model.PhoneNumber;
                editUser.UserType = model.UserType;
                editUser.Active = model.Active;

                // Update the ApplicationUser
                var result = UserManager.Update(editUser);

                //if update succeeded
                if (result.Succeeded)
                {
                    // if the usertype changed, the UserRole has to be changed too
                    if (changeRole)
                    {
                        // first: remove old role
                        var userRoles = UserManager.GetRoles(editUser.Id);
                        UserManager.RemoveFromRoles(editUser.Id, userRoles.ToArray());

                        // second: add new role
                        switch (editUser.UserType)
                        {
                            case UserType.Reinigungskraft:
                                UserManager.AddToRole(editUser.Id, ConstVariables.RoleCleaner);
                                break;
                            case UserType.Pflegekraft:
                                UserManager.AddToRole(editUser.Id, ConstVariables.RoleNurse);
                                break;
                            case UserType.Arzt:
                                UserManager.AddToRole(editUser.Id, ConstVariables.RoleDoctor);
                                break;
                            case UserType.Administrator:
                                UserManager.AddToRole(editUser.Id, ConstVariables.RoleAdministrator);
                                break;
                            case UserType.Therapeut:
                                UserManager.AddToRole(editUser.Id, ConstVariables.RoleTherapist);
                                break;
                            default:
                                // error-message for alert-statement [KrabsJ]
                                TempData["UnexpectedFailure"] = " Bei der Vergabe der Berechtigungen ist ein Fehler aufgetreten.";
                                return RedirectToAction("Index", "Home");
                        }
                    }
                    // success-message for alert-statement [KrabsJ]
                    TempData["EditUserSuccess"] = " Die Benutzerdetails wurden aktualisiert.";
                    return RedirectToAction("Details", "Account", new {userId = model.Id});
                }
                AddErrors(result);
            }
            // if this point is reached there was a error during update
            // failure-message for alert-statement [KrabsJ]
            TempData["EditUserFailed"] = " Die Änderungen konnten nicht gespeichert werden.";
            return View(model);
        }

        // GET: /Acount/NewInitialPassword
        // returns the View for awarding a new password to an user [KrabsJ]
        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator)]
        public ActionResult NewInitialPassword(int? userId)
        {
            if (userId == null)
                return RedirectToAction("Index");

            var id = userId ?? default(int);
            var userPassword = new NewInitialPasswordViewModel { UserId = id, UserName = UserManager.FindById(id).UserName};
            return View(userPassword);
        }

        // POST: /Acount/NewInitialPassword
        // Awards a new password to an user [KrabsJ]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator)]
        public ActionResult NewInitialPassword(NewInitialPasswordViewModel userPassword, string command)
        {
            if (command.Equals(ConstVariables.AbortButton))
                return RedirectToAction("Details", "Account", new { userId = userPassword.UserId });

            if (ModelState.IsValid)
            {
                string resetToken = UserManager.GeneratePasswordResetToken(userPassword.UserId);
                IdentityResult passwordChangeResult = UserManager.ResetPassword(userPassword.UserId, resetToken, userPassword.Password);
                if (passwordChangeResult.Succeeded)
                {
                    // set initialPassword flag to true [KrabsJ]
                    ApplicationUser user = UserManager.FindById(userPassword.UserId);
                    user.InitialPassword = true;
                    UserManager.Update(user);

                    // success-message for alert-statement [KrabsJ]
                    TempData["NewInitialPasswordSuccess"] = " Das Passwort wurde erfolgreich aktualisiert.";
                    return RedirectToAction("Details", "Account", new { userId = userPassword.UserId });
                }
                AddErrors(passwordChangeResult);
            }
            // failure-message for alert-statement [KrabsJ]
            TempData["NewInitialPasswordFailed"] = " Das neue Passwort konnte nicht gespeichert werden.";
            return View(userPassword);
        }

        // POST: /Acount/Delete
        // Deletes an user if possible [KrabsJ]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator)]
        public ActionResult Delete(int? userId)
        {
            if (userId == null)
                return RedirectToAction("Index");

            var id = userId ?? default(int);
            ApplicationUser deleteUser = UserManager.FindById(id);

            // check if there are dependencies
            Stay dependentStay = db.Stays.Where(s => s.ApplicationUserId == id).FirstOrDefault();
            Treatment dependentTreatment = db.Users.Where(u => u.Id == id).SelectMany(u => u.Treatments).FirstOrDefault();

            // if there is a treatment or stay that is linked to the user, the user can't be deleted
            if (dependentStay != null || dependentTreatment != null )
            {
                // failure-message for alert-statement [KrabsJ]
                TempData["DeleteFailed"] = " Es bestehen Abhängigkeiten zu anderen Krankenhausdaten.";
                return RedirectToAction("Details", "Account", new { userId = deleteUser.Id });
            }

            // delete user if there are no dependencies
            try
            {
                var deleteResult = UserManager.Delete(deleteUser);
                if (deleteResult.Succeeded)
                {
                    // success-message for alert-statement [KrabsJ]
                    TempData["DeleteSuccess"] = " Der Benutzer \"" + deleteUser.UserName + "\" wurde erfolgreich gelöscht.";
                    return RedirectToAction("Index", "Account");
                }
                AddErrors(deleteResult);
                // failure-message for alert-statement [KrabsJ]
                TempData["DeleteFailed"] = " Unbekannter Fehler beim Löschen.";
                return RedirectToAction("Details", "Account", new { userId = deleteUser.Id });
            }
            catch (System.Exception)
            {
                // failure-message for alert-statement [KrabsJ]
                TempData["DeleteFailed"] = " Unbekannter Fehler beim Löschen.";
                return RedirectToAction("Details", "Account", new { userId = deleteUser.Id });
            }
        }

        // show the AccountDetails of the logged in user [KrabsJ]
        public ActionResult UserDetails()
        {
            int userId = User.Identity.GetUserId<int>();
            ApplicationUser loggedInUser = UserManager.FindById(userId);
            if (loggedInUser == null)
            {
                // failure
                RedirectToAction("Login");
            }
            return View(loggedInUser);
        }

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
                return View(model);

            // Anmeldefehler werden bezüglich einer Kontosperre nicht gezählt.
            // Wenn Sie aktivieren möchten, dass Kennwortfehler eine Sperre auslösen, ändern Sie in "shouldLockout: true".
            // section changed: login should be based on Username instead of Email and there should be no rememberMe function so it is set to "false" [KrabsJ]
            // var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);

            var requestingUser = UserManager.FindByName(model.Username);
            if (requestingUser != null && requestingUser.Active == false)
            {
                //Added errormessage name to dispay in loginview [HansesM]
                ModelState.AddModelError("InactiveUser",
                    "Der angegebene Nutzer ist inaktiv. Bitte wenden Sie sich an einen Administrator.");
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
            switch (result)
            {
                case SignInStatus.Success:
                    // if there is a returnUrl (this happens if a user who is not logged in tries to call a method), the application should redirect to the required page after login [KrabsJ]
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return RedirectToLocal(returnUrl);
                    else
                        return RedirectToAction("Index", "Home");
                    ;
                case SignInStatus.LockedOut:
                    return View("Lockout");
                // section changed: there should be no rememberMe function --> so it is set to "false" [KrabsJ]
                //case SignInStatus.RequiresVerification:
                //return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                // there is no support of twofactorauthentication in this program [KrabsJ]
                //case SignInStatus.RequiresVerification:
                    //return RedirectToAction("SendCode", new {ReturnUrl = returnUrl, RememberMe = false});

                //Added errormessage name to dispay in loginview [HansesM]
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("WrongUsernameOrPass", "Falscher Benutzername oder falsches Passwort. Falls Sie Ihr Passwort vergessen haben, wenden Sie sich bitte an einen Administrator.");
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
        public async Task<ActionResult> Register(RegisterViewModel model, string command)
        {
            if (command.Equals(ConstVariables.AbortButton))
                return RedirectToAction("Index", "Account");

            if (ModelState.IsValid)
            {
                // create username from surname and lastname
                string username = CreateUserName(model.Surname, model.Lastname);

                //section changed: depending on the expansion of the ApplicationUser class there are more attributes that are necessary to create a new user
                //var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var user = new ApplicationUser
                {
                    UserName = username,
                    Email = model.Email,
                    Title = model.Title,
                    Surname = model.Surname,
                    Lastname = model.Lastname,
                    Active = model.Active,
                    UserType = model.UserType,
                    PhoneNumber = model.PhoneNumber,
                    InitialPassword = true,
                };
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
                            // error-message for alert-statement [KrabsJ]
                            TempData["UnexpectedFailure"] = " Bei der Vergabe der Berechtigungen ist ein Fehler aufgetreten.";
                            return RedirectToAction("Index", "Home");
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
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
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

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }
        #endregion

        // the method generates the userName from surname and lastname [KrabsJ]
        private string CreateUserName(string surname, string lastname)
        {
            // section added: variables for the algorithm of creating the Username [KrabsJ]
            var username = lastname + surname[0];
            string usernameWithNumber;
            ApplicationUser userdb;
            var surnameCounter = surname.Length;
            var userNameNumber = 1;

            //section added: algorithm of creating the Username [KrabsJ]
            //Username should be the Lastname plus the first character of the Surname
            //If there is already another user with the same name the next character of the surname will be added and so on
            //If there is already another user with the same name including the whole lastname plus surname a sequential number will be added
            for (var i = 0; i < surnameCounter; i++)
            {
                userdb = UserManager.FindByName(username);
                if (userdb == null)
                    break;
                if (i + 1 < surnameCounter)
                {
                    username = username + surname[i + 1];
                }
                else
                {
                    usernameWithNumber = username + userNameNumber;
                    while (true)
                    {
                        userdb = UserManager.FindByName(usernameWithNumber);
                        if (userdb == null)
                        {
                            username = usernameWithNumber;
                            break;
                        }
                        userNameNumber++;
                        usernameWithNumber = username + userNameNumber;
                    }
                }
            }
            return username;
        }
    }
}