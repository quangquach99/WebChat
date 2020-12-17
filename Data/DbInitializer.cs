using WebChat.Models;
using System;
using System.Linq;
using WebChat.Data;

namespace WebChat.Data
{
    public static class DbInitializer
    {
        public static void Initialize(WebChatContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            var users = new User[]
            {
                new User {
                    FirstName = "Dean",
                    LastName = "Winchester",
                    PhoneNumber = "0000000001",
                    Email = "DeanWinchester@gmail.com",
                    Password = "deanwinchester",
                    DOB = DateTime.Parse("1989-2-12"),
                    Gender = 0
                },
                new User
                {
                    FirstName = "Marry",
                    LastName = "Winchester",
                    PhoneNumber = "0000000002",
                    Email = "MarryWinchester@gmail.com",
                    Password = "marrywinchester",
                    DOB = DateTime.Parse("1969-2-12"),
                    Gender = 1
                }
            };
            foreach (User s in users)
            {
                context.Users.Add(s);
            }
            context.SaveChanges();

            var profiles = new Profile[]
            {
                new Profile
                {
                    UserID = 1,
                    UserEducation = "College",
                    UserRelationship = 1,
                    UserFacebook = "https://facebook.com/deanwinchester",
                    UserInstagram = "https://instagram.com/deanwinchester",
                    UserTwitter = "https://twitter.com/deanwinchester",
                    UserYoutube = "https://youtube.com.com/deanwinchester",
                    UserAvatar = "DeanWinchester@gmail.com.jpg"
                },
                new Profile
                {
                    UserID = 2,
                    UserEducation = "College",
                    UserRelationship = 2,
                    UserFacebook = "https://facebook.com/marrywinchester",
                    UserInstagram = "https://instagram.com/marrywinchester",
                    UserTwitter = "https://twitter.com/marrywinchester",
                    UserYoutube = "https://youtube.com.com/marrywinchester",
                    UserAvatar = "MarryWinchester@gmail.com.jpg"
                }
            };
            foreach (Profile e in profiles)
            {
                context.Profiles.Add(e);
            }
            context.SaveChanges();

            var conversations = new Conversation[]
            {
                new Conversation
                {
                    ConversationType = 1,
                    CreatedDate = DateTime.Parse("2020-12-12 04:36:00")
                }
            };
            foreach (Conversation e in conversations)
            {
                context.Conversations.Add(e);
            }
            context.SaveChanges();

            var userConversations = new UserConversation[]
            {
                new UserConversation
                {
                    ConversationID = 1,
                    UserID = 1,
                    UserSeen = 0
                },
                new UserConversation
                {
                    ConversationID = 1,
                    UserID = 2,
                    UserSeen = 1
                }
            };
            foreach (UserConversation e in userConversations)
            {
                context.UserConversations.Add(e);
            }
            context.SaveChanges();

            var messages = new Message[]
            {
                new Message
                {
                    MessageText = "Hi Mom!!!",
                    SentTime = DateTime.Parse("2020-12-12 04:36:00")
                },
                new Message
                {
                    MessageText = "Hi Dean!!!",
                    SentTime = DateTime.Parse("2020-12-12 04:37:00")
                }
            };
            foreach (Message e in messages)
            {
                context.Messages.Add(e);
            }
            context.SaveChanges();

            var messageConversations = new MessageConversation[]
            {
                new MessageConversation
                {
                    MessageID = 1,
                    ConversationID = 1
                },
                new MessageConversation
                {
                    MessageID = 2,
                    ConversationID = 1
                }
            };
            foreach (MessageConversation e in messageConversations)
            {
                context.MessageConversations.Add(e);
            }
            context.SaveChanges();

            var senders = new Sender[]
            {
                new Sender
                {
                    UserID = 1,
                    MessageID = 1
                },
                new Sender
                {
                    UserID = 2,
                    MessageID = 2
                }
            };
            foreach (Sender e in senders)
            {
                context.Senders.Add(e);
            }
            context.SaveChanges();
        }
    }
}