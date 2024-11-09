using GroupChat.Data;
using GroupChat.Models;
using GroupChat.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace GroupChat.Controllers
{
    public class ClanUserService: Controller
    {
        private readonly ChatContext _context;

        public ClanUserService(ChatContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Сервис выполняющий вход в чат клана. 
        /// </summary>
        /// <param name="enterToChat"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<EnterToClanReturnDto> EnterToClanChat([FromBody] EnterToChatDto enterToChat)
        {
            if (enterToChat == null || string.IsNullOrEmpty(enterToChat.ClanName) || string.IsNullOrEmpty(enterToChat.UserName))
            {
                return new EnterToClanReturnDto() { StatusCode = 500, Message = "Никнейм и название клана должны быть заданы" };
            }

            var clanUser = _context.ClanUsers.FirstOrDefault(gp => gp.UserName == enterToChat.UserName);
            var clan = _context.Clans.FirstOrDefault(c => c.ClanName == enterToChat.ClanName);

            if (clanUser != null)
            {
                //Если пользователь существует и состоит в клане, но пытается зайти в другой - выдаем ошибку
                if(clan == null || clanUser.ClanId != clan.Id)
                    return new EnterToClanReturnDto() { StatusCode = 500, Message = "Пользователь состоит в другом клане" };

                return new EnterToClanReturnDto() { StatusCode = 200, Message = $"Вы успешно вошли в чат клана {enterToChat.ClanName}" };
            }

            //создаем клан, если его еще нет
            if (clan == null)
            {
                _context.Clans.Add(new Clan { ClanName = enterToChat.ClanName });
                _context.SaveChanges();
            }

            var clanForUser = _context.Clans.First(c => c.ClanName == enterToChat.ClanName);

            _context.ClanUsers.Add(new ClanUser { UserName = enterToChat.UserName, ClanId = clanForUser.Id });

            _context.SaveChanges();

            return new EnterToClanReturnDto() { StatusCode = 200, Message = $"Вы успешно вошли в чат клана {enterToChat.ClanName}" };
        }
    }
}
