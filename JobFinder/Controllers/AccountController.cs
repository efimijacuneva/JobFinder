using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using JobFinder.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.EntityFramework;

namespace JobFinder.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController() { }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get => _signInManager
                   ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            private set => _signInManager = value;
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager
                   ?? HttpContext.GetOwinContext()
                                 .GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        
        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await SignInManager
                .PasswordSignInAsync(
                    model.Email, model.Password,
                    model.RememberMe, shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction(
                        "SendCode", new { ReturnUrl = returnUrl, model.RememberMe });
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        
        [AllowAnonymous]
        public ActionResult Register()
        {
            
            ViewBag.RoleList = new SelectList(
                new[] { "Candidate", "Company" });
            return View();
        }

        
        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.RoleList = new SelectList(
                    new[] { "Candidate", "Company" });
                return View(model);
            }

           
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Role = model.Role
            };

            var result = await UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                
                try
                {
                    
                    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new JobFinderContext()));
                    if (!roleManager.RoleExists(model.Role))
                    {
                        roleManager.Create(new IdentityRole(model.Role));
                    }

                    
                    if (!await UserManager.IsInRoleAsync(user.Id, model.Role))
                    {
                        await UserManager.AddToRoleAsync(user.Id, model.Role);
                    }

                   
                    await SignInManager.SignInAsync(
                        user, isPersistent: false, rememberBrowser: false);

                    TempData["Success"] = $"Welcome! You have successfully registered as a {model.Role}.";
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    
                    ModelState.AddModelError("", "User created but role assignment failed. Please contact administrator.");
                    return View(model);
                }
            }

            AddErrors(result);
            ViewBag.RoleList = new SelectList(
                new[] { "Candidate", "Company" });
            return View(model);
        }

       
        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public ActionResult LogOff()
        {
            HttpContext.GetOwinContext()
                       .Authentication.SignOut(
                           DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var e in result.Errors)
                ModelState.AddModelError("", e);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}
