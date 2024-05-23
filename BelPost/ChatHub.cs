using BelPost.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using BelPost.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace BelPost
{
    public class Chat : Hub
    {
        private readonly ApplicationContext _context;
        public Chat(ApplicationContext context)
        {
            _context = context;
        }
        //public async Task SendMessage(string user, string message)
        //{
        //    Clients.All.SendAsync("ReceiveMessage", user, message);
        //}
        [Authorize]
        public async Task SendMessage(string IdReceiver, string message)
        {
            string IdSender = Context.UserIdentifier;

            var dialog = _context.Dialogs.FirstOrDefault(d => d.IdSender == IdSender && d.IdReceiver == IdReceiver) ??
                         _context.Dialogs.FirstOrDefault(d => d.IdSender == IdReceiver && d.IdReceiver == IdSender);

            if (dialog == null)
            {
                dialog = new Dialog
                {
                    IdSender = IdSender,
                    IdReceiver = IdReceiver
                };
                _context.Dialogs.Add(dialog);
               await _context.SaveChangesAsync();
            }

            var messageEntity = new Message
            {
                DialogId = dialog.Id,
                SenderId = IdSender,
                Text = message,
                SentAt = DateTime.Now
            };
            string user = _context.Users.FirstOrDefault(u => u.Id == IdSender).FirstName.ToString();
            _context.Messages.Add(messageEntity);
           await _context.SaveChangesAsync();
           await Clients.Group($"Dialog_{dialog.Id}").SendAsync("ReceiveMessage", user, message, DateTime.Now.ToString("HH:mm"));
            

        }

        public async Task LoadHistory(string recipientId)
        {
            string IdSender = Context.UserIdentifier;

            var dialog = _context.Dialogs.FirstOrDefault(d => d.IdSender == IdSender && d.IdReceiver == recipientId) ??
                         _context.Dialogs.FirstOrDefault(d => d.IdSender == recipientId && d.IdReceiver == IdSender);

            if (dialog != null)
            {
                var messages = _context.Messages
     .Where(m => m.DialogId == dialog.Id)
     .OrderBy(m => m.SentAt)
     .Join(_context.Users, m => m.SenderId, u => u.Id, (m, u) => new MessageViewModel
     {
         SenderId = m.SenderId,
         SenderName = u.FirstName, // Получаем FirstName из таблицы Users
         Text = m.Text,
         SentAt = m.SentAt.ToString("HH:mm")
     })
     .ToList();

                await Clients.Caller.SendAsync("LoadHistory", messages);
            }
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            // Добавить пользователя к группам для его диалогов
            var dialogs = _context.Dialogs.Where(d => d.IdSender == userId || d.IdReceiver == userId);
            foreach (var dialog in dialogs)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"Dialog_{dialog.Id}");
            }

            await base.OnConnectedAsync();
        }
        //public async Task SendMessage(string group, string sender, string receiver, string message)
        //{
        //    // Сохраните сообщение в базе данных

        //    Message message1 = new Message
        //    {
        //        messenge = message,
        //        Sender = sender,
        //        Time = DateTime.Now
        //    };
        //    _context.Messages.Add(message1);
        //    await _context.SaveChangesAsync();

        //    // Передайте сообщение через SignalR
        //    await Clients.Group(group).SendAsync("ReceiveMessage", sender, message);
        //}

        //public async Task Messenge(string message, string UserName)
        //{
        //    User user = await _userManager.FindByNameAsync(UserName);
        //    Message messenge = new Message { Sender = UserName, Time = DateTime.Now, messenge = message, UrlImg = user.UrlImg };
        //    _context.Messages.Add(messenge);
        //    await _context.SaveChangesAsync();
        //    var messenges = _context.Messages.OrderBy(x => x.Time).ToList();
        //    await Clients.All.SendAsync("getMessenge", messenges);
        //}
        //public async Task GetUser(string userName)
        //{
        //    if (!Users.Any(x => x.UserName == userName))
        //    {
        //        User user = await _userManager.FindByNameAsync(userName);
        //        Users.Add(user);
        //    }
        //    await Clients.All.SendAsync("getUsers", Users);
        //}
        //public override async Task OnConnectedAsync()
        //{
        //    await this.Clients.Caller.SendAsync("getConnected");
        //    await base.OnConnectedAsync();
        //}
        //public override async Task OnDisconnectedAsync(Exception exception) 
        //{
        //    User user = Users.FirstOrDefault(x => x.UserName == Context.User.Identity.Name);
        //    Users.Remove(user);
        //    await Clients.All.SendAsync("getUsers", Users);
        //    await base.OnDisconnectedAsync(exception);
        //}



    }
}