using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebChat.Data;
using WebChat.GeneralClass;
using WebChat.Models;

namespace WebChat.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WebChatContext _context;

        public HomeController(ILogger<HomeController> logger, WebChatContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("userId") == null)
            {
                return RedirectToAction("Index", "Registration");
            }
            else
            {
                // Get Current User ID
                var currentUserID = HttpContext.Session.GetInt32("userId");
                // Get The Last Conversation ID
                var lastConversationID = _context.Messages
                    .OrderByDescending(m => m.SentTime)
                    .FirstOrDefault(m => m.UserID == currentUserID)
                    .ConversationID;
                ViewData["newestConversationID"] = lastConversationID;
                return View();
            }            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
