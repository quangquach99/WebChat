using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebChat.Data;
using WebChat.GeneralClass;

namespace WebChat.Controllers
{
    public class MessengerController : Controller
    {
        private readonly WebChatContext _context;
        public MessengerController(WebChatContext context)
        {
            _context = context;
        }
        [ServiceFilter(typeof(SessionCheck))]
        public async Task<IActionResult> IndexAsync()
        {
            var UserID = HttpContext.Session.GetInt32("userId");
            var user = await _context.Users
                .Include(u => u.Profile)
                .Include(uc => uc.UserConversations)
                    .ThenInclude(c => c.Conversation)
                        .ThenInclude(mc => mc.MessageConversations)
                            .ThenInclude(m => m.Message)
                                .ThenInclude(s => s.Sender)
                                .FirstOrDefaultAsync(u => u.ID == UserID);
            return View(user);
        }
    }
}
