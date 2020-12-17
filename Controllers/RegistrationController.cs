using System.Threading.Tasks;
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
