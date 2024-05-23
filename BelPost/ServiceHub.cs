
using BelPost.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace BelPost
{
    public class ServiceHub : Hub
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationContext _context;
        public ServiceHub(ApplicationContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        
       
        public override async Task OnConnectedAsync()
        {
            await this.Clients.Caller.SendAsync("getConnected");
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await this.Clients.Caller.SendAsync("DelConnected");
            await base.OnDisconnectedAsync(exception);
        }
        
    }
}
