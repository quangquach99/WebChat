using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebChat.Data;
using WebChat.Models;

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
            var listAccount = _context.Accounts.ToList();
            return RedirectToAction("index","Home");
        }
        public IActionResult Register(User user)
        {
            var listAccount = _context.Users.ToList();
            return RedirectToAction("index", "Home");
        }
    }
}
