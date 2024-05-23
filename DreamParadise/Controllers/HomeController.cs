using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DreamParadise.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DreamParadise.Controllers;

 public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MyContext _context;

        public HomeController(ILogger<HomeController> logger, MyContext context)
        {
            _logger = logger;
            _context = context;
        }




    //*================ Home  view  action =============
        public IActionResult Index()
        {
            return View();
        }


    //*================ About  view  action =============


          public IActionResult About()
        {
            return View();
        }

    //*================ Rooms  view  action =============

          public IActionResult Rooms()
        {
            return View();
        }

    //*================ Book now   view  action =============
        [SessionCheck]
         public IActionResult Booking()
        {
            return View();
        }


    //*================ Contact Us  view  action =============

         public IActionResult ContactUs()
        {
            return View();
        }



    //*================ Reservations  view  action =============
        [SessionCheck]
         public IActionResult Reservations()
        {
            return View();
        }

         [HttpPost("Reservations/new")]
        public IActionResult CreateReservation (Reservation newReservation)
        {    
            if(ModelState.IsValid)
            {
                _context.Add(newReservation);    
                _context.SaveChanges();
                return RedirectToAction("Reservations");
            } 
            else 
            {
                return View ("Booking");    
            }
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
                context.Result = new RedirectToActionResult("LogReg", "LogReg", null);
            }
        }
    }
}
