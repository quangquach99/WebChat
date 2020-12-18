using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebChat.GeneralClass;
using WebChat.Models;

namespace WebChat.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly WebChat.Data.WebChatContext _context;

        public RegistrationController(WebChat.Data.WebChatContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("Email,Password")] User user)
        {
                User objAccount =  _context.Users.FirstOrDefault(ac => ac.Email == user.Email);
                if(objAccount != null)
                {
                    CEncryptor hash = new CEncryptor();
                    if (hash.MD5Hash(user.Password) == objAccount.Password)
                    {
                        HttpContext.Session.SetString("userFullname", objAccount.FullName.ToString());
                        HttpContext.Session.SetString("userEmail", user.Email.ToString());
                        HttpContext.Session.SetInt32("userId", objAccount.ID);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                    // Alert Wrong Password
                        return View("Index",user);
                    }
                }
                else
                {
                    // Alert Account Does Not Exist
                    return View("Index",user);
                } 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(
            [Bind("FirstName,LastName,PhoneNumber,Email,Password,DOB,Gender")] User user)
        {
            if (ModelState.IsValid)
            {
                User existedEmail = await _context.Users.FirstOrDefaultAsync(e => e.Email == user.Email);
                if(existedEmail != null)
                {
                    ViewData["existedEmail"] = "This Email Has Been Taken!";
                    return View("Index", existedEmail);
                }
                CEncryptor hash = new CEncryptor();
                user.Password = hash.MD5Hash(user.Password);
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                TempData["success"] = "Created Successfully.";
                return RedirectToAction("Index");
            }

            return View("Index");
        }
    }
}
