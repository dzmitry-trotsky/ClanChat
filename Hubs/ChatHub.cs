using GroupChat.Controllers;
using GroupChat.Models.Dto;
using Microsoft.AspNetCore.SignalR;

namespace GroupChat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ClanUserService _userService;
        private readonly MessageService _messageService;

        public ChatHub(ClanUserService userService, MessageService messageService)
        {
            _userService = userService;
            _messageService = messageService;
        }
        /// <summary>
        /// Проверяет состоит ли пользоваетль в клане ии нет
        /// Если нет - выдаем соотв. соощение, историю соощений клана не показываем, бокируем отправку
        /// Если состоит - выводим последние 50 сообщений и разрешаем отправлять.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="clanName"></param>
        /// <returns></returns>
        public async Task Enter(string username, string clanName)
        {
            var res = await _userService.EnterToClanChat(new EnterToChatDto { UserName = username, ClanName = clanName });

            if (res != null) 
            {
                if (res.StatusCode > 400)
                {
                    await Clients.Caller.SendAsync("ErrorMessage", res.Message);
                    return;
                }
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, clanName);
            await Clients.Group(clanName).SendAsync("Notify", $"{username} вошел в чат");

            var lastMessages = await _messageService.GetLastByClanName(clanName);
            await Clients.Client(username).SendAsync("ReceiveLastMessages", lastMessages);
            await Clients.Caller.SendAsync("ReceiveLastMessagesCaller", lastMessages);
        }
        public async Task Send(string message, string userName, string clanName)
        {
            await _messageService.Create(new MessageDto() 
                { 
                    Message = message, 
                    SenderName = userName, 
                    SendDate = DateTime.Now, 
                    ClanName = clanName 
                });

            await Clients.Group(clanName).SendAsync("Receive", message, userName);
        }
    }
}
