﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebChat.Data;
using WebChat.GeneralClass;
using WebChat.Models;
using WebChat.Models.WebChatViewModel;

namespace WebChat.Controllers
{
    [ServiceFilter(typeof(SessionCheck))]
    public class MessengerController : Controller
    {
        private readonly WebChatContext _context;
        public MessengerController(WebChatContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> HomeAsync(int ID)
        {
            // Current User
            var UserID = HttpContext.Session.GetInt32("userId");
            // CHECK IF CURRENT USER IS IN THE CONVERSATION
            var checkUserConversation = _context.UserConversations.
                FirstOrDefault(cuc => cuc.ConversationID == ID && cuc.UserID == UserID);
            if (checkUserConversation == null)
            {
                return NotFound();
            }
            var user = await _context.Users
                .Include(p => p.Profile)
                .Include(uc => uc.UserConversations)
                    .ThenInclude(ucu => ucu.User)
                        .ThenInclude(ucup => ucup.Profile)
                .FirstOrDefaultAsync(u => u.ID == UserID);
            return View(user);
        }
        public List<ConversationViewModel> ConVMs = new List<ConversationViewModel>();
        public List<MessagesViewModel> MessVMs = new List<MessagesViewModel>();

        // GET ALL CONVERSATION FOR PARTICULAR USER
        public async Task<IActionResult> GetConversations()
        {
            // Current User
            var UserID = HttpContext.Session.GetInt32("userId");
            // Get All UserConversations Related To This Current User
            var currentUserConversations = _context.UserConversations.Where(uc => uc.UserID == UserID).ToList();
            foreach (var i in currentUserConversations)
            {
            // View Model
            // UserId That Related To The Conversation with the current user
                var userId = _context.UserConversations.
                    FirstOrDefault(u => (u.UserID != UserID && u.ConversationID == i.ConversationID)).UserID;
                var userSeen = _context.UserConversations.
                    FirstOrDefault(u => (u.UserID != UserID && u.ConversationID == i.ConversationID)).UserSeen;
                var UserFullName = _context.Users.FirstOrDefault(u => u.ID == userId).FullName;
                var UserAvatar = _context.Users.Include(u => u.Profile)
                    .FirstOrDefault(u => u.ID == userId).Profile.UserAvatar;
                ConVMs.Add(
                    new ConversationViewModel 
                    { 
                        ConversationID = i.ConversationID,
                        UserFullName = UserFullName, 
                        UserAvatar = UserAvatar
                    });
            }
            return Json(ConVMs);
        }
        // GET ALL MESSAGES FROM PARTICULAR CONVERSATION
        public async Task<IActionResult> Conversation(int ID)
        {
            // Current User
            var UserID = HttpContext.Session.GetInt32("userId");
            //var UserID = 1;
            // CHECK IF CURRENT USER IS IN THE CONVERSATION
            var checkUserConversation = _context.UserConversations.
                FirstOrDefault(cuc => cuc.ConversationID == ID && cuc.UserID == UserID);
            if (checkUserConversation == null)
            {
                return NotFound();
            }
            // GET ALL MESSAGES
            var messagesList = _context.Messages.Where(m => m.ConversationID == ID).ToList();
            foreach (var message in messagesList)
            {
                var senderID = message.UserID;
                var senderAvatar = _context.Profiles.FirstOrDefault(p => p.UserID == senderID).UserAvatar;
                var messageText = message.MessageText;
                var sentTime = message.SentTime;
                MessVMs.Add(
                    new MessagesViewModel
                    {
                        ConversationID = ID,
                        SenderID = senderID,
                        SenderAvatar = senderAvatar,
                        SenderMessage = messageText,
                        MessagedTime = sentTime
                    });
            }
            if(messagesList == null)
            {
                return Json(1);
            }
            return Json(MessVMs);
        }

        //public bool InitialMessages()
        //{
        //    Message message = new Message()
        //    {
        //        MessageText = "How Are You?",
        //        SentTime = System.DateTime.Now,
        //        ConversationID = 1,
        //        UserID = 1
        //    };
        //    _context.Messages.Add(message);
        //    _context.SaveChanges();
        //    message = new Message()
        //    {
        //        MessageText = "Are You Still Good?",
        //        SentTime = System.DateTime.Now.AddMinutes(1),
        //        ConversationID = 1,
        //        UserID = 1
        //    };
        //    _context.Messages.Add(message);
        //    _context.SaveChanges();
        //    message = new Message()
        //    {
        //        MessageText = "Im just fine",
        //        SentTime = System.DateTime.Now.AddMinutes(2),
        //        ConversationID = 1,
        //        UserID = 2
        //    };
        //    _context.Messages.Add(message);
        //    _context.SaveChanges();
        //    message = new Message()
        //    {
        //        MessageText = "How about you",
        //        SentTime = System.DateTime.Now.AddMinutes(1),
        //        ConversationID = 1,
        //        UserID = 2
        //    };
        //    _context.Messages.Add(message);
        //    _context.SaveChanges();
        //    return true;
        //}

        // CREATE NEW CONVERSATION
        [HttpPost]
        public async Task<IActionResult> NewConversationAsync(int userId)
        {
            // Get UserID Of Sender
            var currentUserId = HttpContext.Session.GetInt32("userId");
            // Check If Receiver Existed
            var user = _context.Users.FirstOrDefault(u => u.ID == userId);
            if (user != null)
            {
                // Check If Conversation Already Existed
                var userConversationIdList = _context.UserConversations
                    .Where(u => u.UserID == currentUserId).ToList();
                foreach (var i in userConversationIdList)
                {
                    var uConversation = _context.UserConversations
                        .Include(u => u.Conversation)
                        .Where(u => u.ConversationID == i.ConversationID).ToList();
                    foreach (var j in uConversation)
                    {
                        if (j.UserID == userId && j.Conversation.ConversationType == 0)
                        {
                            return Json(1);
                        }
                    }
                }
                // Create New Conversation
                Conversation conversation = new Conversation();
                DateTime currentTime = System.DateTime.Now;
                conversation.CreatedDate = currentTime;
                conversation.ConversationType = 0;
                _context.Add(conversation);
                await _context.SaveChangesAsync();
                // Create New UserConversations
                // Get Conversation ID
                var conversationId = _context.Conversations
                    .FirstOrDefault(c => c.CreatedDate == currentTime).ID;
                // Save User Conversations
                UserConversation uC1 = new UserConversation();
                uC1.ConversationID = conversationId;
                uC1.UserID = (int)currentUserId;
                uC1.UserSeen = 1;
                _context.Add(uC1);
                await _context.SaveChangesAsync();

                UserConversation uC2 = new UserConversation();
                uC2.ConversationID = conversationId;
                uC2.UserID = userId;
                uC2.UserSeen = 0;
                _context.Add(uC2);
                await _context.SaveChangesAsync();

                return Ok();
            }
            return NotFound();
        }
    }
}
