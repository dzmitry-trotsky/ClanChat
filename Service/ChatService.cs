using GroupChat.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GroupChat.Controllers
{
    public class ChatService: Controller
    {
        private readonly ChatContext _context;

        public ChatService(ChatContext context)
        {
            _context = context;
        }
    }
}
