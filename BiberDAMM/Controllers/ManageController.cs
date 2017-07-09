using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BiberDAMM.Models;
using BiberDAMM.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using BiberDAMM.Helpers;

namespace BiberDAMM.Controllers
{
    //[KrabsJ]
    [CustomAuthorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get => _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            private set => _signInManager = value;
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model, string command)
        {
            // return to details page without changes if abortbutton was clicked [KrabsJ]
            if (command.Equals(ConstVariables.AbortButton))
                return RedirectToAction("UserDetails", "Account");

            if (!ModelState.IsValid)
            {
                // failure message for alert-statement [KrabsJ]
                TempData["ChangePasswordFailed"] = " Das neue Passwort konnte nicht gespeichert werden.";
                return View(model);
            }
            // Change PrimaryKey of identity package to int [var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);] //KrabsJ
            var result =
                await UserManager.ChangePasswordAsync(User.Identity.GetUserId<int>(), model.OldPassword,
                    model.NewPassword);
            if (result.Succeeded)
            {
                // Change PrimaryKey of identity package to int [var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());] //KrabsJ
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
                if (user != null)
                {
                    // set initialPassword flag to false [KrabsJ]
                    user.InitialPassword = false;
                    UserManager.Update(user);

                    await SignInManager.SignInAsync(user, false, false);

                    // success message for alert-statement [KrabsJ]
                    TempData["ChangePasswordSuccess"] = " Das Passwort wurde erfolgreich geändert.";
                    return RedirectToAction("UserDetails", "Account");
                }
            }
            AddErrors(result);
            // failure message for alert-statement [KrabsJ]
            TempData["ChangePasswordFailed"] = " Das neue Passwort konnte nicht gespeichert werden.";
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Hilfsprogramme

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error);
        }
        #endregion
    }
}