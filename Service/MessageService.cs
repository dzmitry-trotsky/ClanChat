using AutoMapper;
using GroupChat.Data;
using GroupChat.Models;
using GroupChat.Models.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GroupChat.Controllers
{
    public class MessageService: Controller
    {
        private readonly ChatContext _context;
        private IMapper _mapper;

        public MessageService(ChatContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task Create([FromBody] MessageDto message)
        {
            var clan = await _context.Clans.FirstOrDefaultAsync(c => c.ClanName == message.ClanName);

            if (clan != null)
            {
                var newMessage = new Message 
                { 
                    SenderName = message.SenderName, 
                    Text = message.Message, 
                    ClanId = clan.Id, 
                    SendDate = DateTime.Now 
                };

                _context.Messages.Add(newMessage);
                await _context.SaveChangesAsync();
            }
        }


        [HttpGet]
        public async Task<List<MessageReturnDto>> GetLastByClanName(string clanName)
        {
            var clan = _context.Clans.FirstOrDefault(c => c.ClanName == clanName);

            if(clan == null)
                return [];

            var messages = await _context.Messages.Where(m => m.ClanId == clan.Id)
                .OrderBy(m => m.SendDate).Take(50).ToListAsync();

            var messagesToReturn = _mapper.Map<List<MessageReturnDto>>(messages);

            return messagesToReturn;
        }
    }
}
