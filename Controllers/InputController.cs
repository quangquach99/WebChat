using Microsoft.AspNetCore.Mvc;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebChat.Data;
using WebChat.Models;
using WebChat.GeneralClass;
using Microsoft.AspNetCore.Http;
using System;

namespace WebChat.Controllers
{
    public class InputController : Controller
    {
        private readonly WebChatContext _context;
        public InputController(WebChatContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]

        public IActionResult Login(Account ac)
        {
            if (ModelState.IsValid)
            {
                var objAccount = _context.Accounts.ToList().FirstOrDefault(a => a.UserEmail == ac.UserEmail);
                if (objAccount != null)
                {
                    if(CEncryptor.MD5Hash(ac.UserPassword) == objAccount.UserPassword)
                    {
                        CookieOptions acCookie = new CookieOptions();
                        acCookie.Expires = DateTime.Now.AddMilliseconds(1000);
                        Response.Cookies.Append(ac.UserEmail.ToString(), ac.UserPassword.ToString(), acCookie);
                        HttpContext.Session.SetString(ac.UserEmail.ToString(), ac.UserPassword.ToString());
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {                      
                        return View("Index");
                    }
                }
                else
                {             
                    return View("Index");
                }
            }
            else
            {
                return View("Index");
            }
        }

        public IActionResult Register(User user)
        {
            var listAccount = _context.Users.ToList();
            return RedirectToAction("index", "Home");
        }
    }
}
