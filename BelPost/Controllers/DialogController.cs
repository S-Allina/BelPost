using BelPost.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace BelPost.Controllers
{
    public class DialogController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<User> _userManager;


        public DialogController(ApplicationContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> DialogUser()
        {
            string IdUser = _userManager.GetUserAsync(HttpContext.User).Result.Id;
            var users = await _context.ProfileView
     .Join(
         _context.Dialogs
             .Where(d => d.IdSender == IdUser || d.IdReceiver == IdUser)
             .Select(d => d.IdSender == IdUser ? d.IdReceiver : d.IdSender)
             .Distinct(),
         u => u.Id,
         userId => userId,
         (u, userId) => u
     )
     .ToListAsync();
            ViewBag.IdUser=IdUser;
            return View(users);
        }

        [Authorize]
        public async Task<IActionResult> IndexDialog(string IdReceiver)
        {

            var receiver =await _context.Users.FirstOrDefaultAsync(u=>u.Id== IdReceiver);
            return View(receiver);
        }
       
    }
}
