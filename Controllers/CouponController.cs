using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RedBeltExam.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;

namespace RedBeltExam.Controllers;


[SessionCheck]
public class CouponController : Controller
{
    private readonly ILogger<CouponController> _logger;
    private MyContext _context; //DO NOT FORGET THIS 

    public CouponController(ILogger<CouponController> logger, MyContext context) //DO NOT FORGET TO ADD AFTER THE , 
    {
        _logger = logger;
        _context = context; //DO NOT FORGET TO ADD THIS
    }




//Route that will render the dashboard
    [HttpGet("coupons")]
    public IActionResult Dashboard()
    {
        List<Coupon> allCoupons = _context.Coupons.Include(c => c.Couponer).Include(c => c.UsersConfirmed).ToList();
        return View(allCoupons);
    }



//route that will render the view (form to submit)
    [HttpGet("coupons/new")]
    public ViewResult NewCoupon()
    {
        return View();
    }


//Route that will post the form into our db
    [HttpPost("coupons/create")]
    public IActionResult CreateCoupon(Coupon newCoupon)
    {
        if (!ModelState.IsValid)
        {
            return View("NewCoupon");
        }
        newCoupon.UserId = (int)HttpContext.Session.GetInt32("UniqueUserID");
        _context.Add(newCoupon);
        _context.SaveChanges();
        return RedirectToAction("Dashboard");
    }


//route for toggle (Used Coupon)
    [HttpPost("coupons/{id}/usedcoupons")]
    public RedirectToActionResult ToggleUsed(int id)
    {
        int UniqueUserID = (int)HttpContext.Session.GetInt32("UniqueUserID");
        UserCouponerUsed existingUsed = _context.UserCouponerUseds.FirstOrDefault(c => c.CouponId == id && c.UserId == UniqueUserID);
        if (existingUsed == null) //if it does not already exists, we will create it
        {
            UserCouponerUsed newUsed = new()
            {
                UserId = UniqueUserID,
                CouponId = id
            };
            _context.Add(newUsed);
        }
        // else //if we did find it, meaning they already used it
        // {
        //     _context.Remove(existingUsed);
        // }
        _context.SaveChanges();
        return RedirectToAction("Dashboard");
    }



//route for view one user's coupons
[HttpGet("coupons/users")]
public IActionResult UserCoupons()

{
    int UniqueUserID = (int)HttpContext.Session.GetInt32("UniqueUserID");
    User? oneUser = _context.Users
        .Include(u => u.Sales)
        .Include(u => u.ConfirmedCoupons)
        // .ThenInclude(ucu => ucu.CouponUsed)
        .FirstOrDefault(u => u.UserId == UniqueUserID);

    if (oneUser == null)
    {
        return RedirectToAction("Dashboard"); //notes: redirect to index page
    }
    return View("UserCoupons", oneUser);
}



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}