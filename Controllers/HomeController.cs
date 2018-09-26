using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using bank.Models;
using System.Linq;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;




namespace bank.Controllers {
    public class HomeController : Controller {
        private MyContext _context;
        public HomeController(MyContext context) {
            _context = context;
        }
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
           
            return View();
        }

        [HttpPost]
        [Route("login")]
        public IActionResult login(loginVal newLogin)
        {
            if(ModelState.IsValid)
            {
            var user = _context.user.Where(a => a.UserName == newLogin.UserNameLogin).SingleOrDefault();
            if(user == null)
            {
                ViewBag.Error="Invalid Login";
                return View("Index");
            }
            if(user != null && newLogin.passwordLogin != null)
            {
                var Hasher = new PasswordHasher<User>();
                
                if(0 != Hasher.VerifyHashedPassword(user, user.Password, newLogin.passwordLogin))
                {
                   HttpContext.Session.SetInt32("User_id", user.User_id);
                    return RedirectToAction("Dashboard");
                }
            }
            }
            ViewBag.Error="Invalid Ligin";
            return View("Index");
        }
        [HttpPost]
        [Route("register")]
        public IActionResult Register(User myreg)
        {
           
            
            if(ModelState.IsValid)
            {
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                myreg.Password = Hasher.HashPassword(myreg, myreg.Password);
                myreg.Wallet = 1000;
                 _context.user.Add(myreg);
                 _context.SaveChanges();
                 List<User> allreg = _context.user.Where(a => a.Email == myreg.Email).ToList();
                 HttpContext.Session.SetInt32("User_id", allreg[0].User_id);
                 
                 
                 return RedirectToAction("Dashboard");
            }
            return View("Index"); 
            
        }
        [HttpGet]
        [Route("Dashboard")]
        public IActionResult Dashboard()
        {
            if(HttpContext.Session.GetInt32("User_id") == null) {
                return RedirectToAction("Index");
            }
            User allreg = _context.user.Include(user => user.Bid).SingleOrDefault(a => a.User_id == HttpContext.Session.GetInt32("User_id"));
            List<Function> AllFunctions = _context.function
                                        .Include(a => a.bid)
                                        .Include(a => a.user)
                                        .ToList();
            List<Key> UserLikes = _context.key.Where(c => c.user.Equals(allreg)).ToList();
            ViewBag.User_id = HttpContext.Session.GetInt32("User_id");
            ViewBag.AllFunctions = AllFunctions;
            ViewBag.CurrentUser = allreg;
            ViewBag.User_id = HttpContext.Session.GetInt32("User_id");
            ViewBag.UserLike = UserLikes;
            return View();
        }
        [HttpGet]
        [Route("Stats/{Functionid}")]
        public IActionResult Info(int Functionid){
            Function CurrentFunction = _context.function.Include(a => a.user).Include(b => b.bid).ThenInclude(c => c.user)
            .SingleOrDefault(a => a.Functionid == Functionid);
            ViewBag.CurrentFunction = CurrentFunction;
            return View("Stats");
        }

        [HttpPost]
        [Route("Idea")]
        public IActionResult Idea(Function myFunction)
        {
            if (myFunction == null || myFunction.Product == null) {
                TempData["idea_error"] = true;
                return RedirectToAction("Dashboard");
            }
            if (myFunction.Product.Length <= 2) {
                TempData["idea_error"] = true;
                return RedirectToAction("Dashboard");
            }
            myFunction.Userid= (int)HttpContext.Session.GetInt32("User_id");
            _context.function.Add(myFunction);
            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }
            


        [HttpPost]
        [Route("Bidding")]
        public IActionResult Money(int Functionid, Key SuperBid) {
            if(HttpContext.Session.GetInt32("User_id") == null) {
                return RedirectToAction("Index", "User");
            }
            User allreg = _context.user.Include(user => user.Bid).SingleOrDefault(a => a.User_id == HttpContext.Session.GetInt32("User_id"));
            List<Function> AllFunctions = _context.function
                                        .Include(a => a.bid)
                                        .Include(a => a.user)
                                        .ToList();
           List<Key> NewBid = _context.key.Where(c => c.user.Equals(allreg)).ToList();
           
           
            _context.key.Add(NewBid[0]);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");                   
        }
              
        [HttpGet]
        [Route("Delete/{Functionid}")]
        public IActionResult Delete(int Functionid) {
            if(HttpContext.Session.GetInt32("User_id") == null) {
                return RedirectToAction("Index", "User");
            }
            Function CurrentFunction = _context.function
                                            .SingleOrDefault(a => a.Functionid == Functionid);
            _context.function.Remove(CurrentFunction);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        [HttpGet]
        [Route("Display")]
        public IActionResult Display(Function Validation)
        {
            var validation = _context.function.Where(a => a.Product == Validation.Product);
            if(validation == null)
            {
                ViewBag.Error="Invalid Login";
                return View("Display");
            }


            return View();
        }
        [HttpPost]
        [Route("NewProduct")]
        public IActionResult NewProduct(Function myFunction)
        {
            myFunction.Userid= (int)HttpContext.Session.GetInt32("User_id");
            _context.function.Add(myFunction);
            _context.SaveChanges();
            return RedirectToAction("Display");
        }
        [HttpGet]
        [Route("/Loggout")]
        public IActionResult Loggout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}