using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebChat.GeneralClass;

namespace WebChat.Controllers
{
    public class MessengerController : Controller
    {
        //private readonly WebChat.Data.WebChatContext _context;
        [ServiceFilter(typeof(SessionCheck))]
        public IActionResult Index()
        {
            return View();
        }
    }
}
