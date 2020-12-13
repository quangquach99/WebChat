using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebChat.Data;

namespace WebChat.Controllers
{
    public class TestChatController : Controller
    {
        private readonly WebChatContext _context;

        public TestChatController(WebChatContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users
                .Include(uc => uc.UserConversations)
                    .ThenInclude(c => c.Conversation)
                        .ThenInclude(mc => mc.MessageConversations)
                            .ThenInclude(m => m.Message)
                                .ThenInclude(s => s.Sender)
                                    .ThenInclude(u => u.User)
                .AsNoTracking()
                .ToListAsync();
            return View(users);
        }
    }
}
