using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DreamParadise.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DreamParadise.Controllers;

 public class LogRegController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MyContext _context;

        public LogRegController(ILogger<HomeController> logger, MyContext context)
        {
            _logger = logger;
            _context = context;
        }




    //*================ Login and registration  view  action =============
        public IActionResult LogReg()
        {
            return View();
        }
      


        
    //*================ Registration with password hashing =============
        [HttpPost("users/create")]
        public IActionResult Register(User newUser)
        {
        if (ModelState.IsValid)
        {
            // Password hashing
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            newUser.Password = hasher.HashPassword(newUser, newUser.Password);

            _context.Add(newUser);
            _context.SaveChanges();
            HttpContext.Session.SetInt32("UserId", newUser.UserId);
            // Redirect to weddings view after successful registration
            return RedirectToAction("Index", "Home");
        }
        else
        {
            return View("LogReg");
        }
        }




    //*================ Login Action  =============
        
        [HttpPost("users/login")]
        public IActionResult Login(LoginUser userSubmission)
        {
            if (ModelState.IsValid)
            {
                User? userInDb = _context.Users.FirstOrDefault(u => u.Email == userSubmission.LoginEmail);

                if (userInDb == null)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid Email/Password");
                    return View("LogReg");
                }

                PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.LoginPassword);

                if (result == PasswordVerificationResult.Success)
                {
                    HttpContext.Session.SetInt32("UserId", userInDb.UserId); // Set session with user ID
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("Password", "Invalid Email/Password");
                    return View("LogReg");
                }
            }
            else
            {
                return View("LogReg");
            }
        }
    
    //*================ Logout Action =============
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("LogReg");
        }










    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }



    //*================ Session check attribute  =============
    public class SessionCheckAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            int? userId = context.HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                context.Result = new RedirectToActionResult("LogReg", "Home", null);
            }
        }
    }
}
