using BelPost.Models;
using BelPost.Service;
using BelPost.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BelPost.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationContext _context;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        } 

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
                User user = new User 
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    FirstName=model.FirstName,
                    LastName=model.LastName,
                    Img = null

                };
                if (model.ImgFromView != null)
                {
                
                user.Img = Services.ConvertPictureForDb(model.ImgFromView); ;

            }

                var result = await _userManager.CreateAsync(user, model.Password);

                await _userManager.AddToRoleAsync(user, "user");

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                Services.SetIdCurrentUser( _userManager.FindByNameAsync(user.UserName).Result.Id);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> LoginAsync()
        {
            var externalProviders = await _signInManager.GetExternalAuthenticationSchemesAsync();
            return View(new LoginViewModel
            {
                ReturnUrl = "Home/Index",
                ExternalProviders = externalProviders
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
         
                var user = await _userManager.FindByNameAsync(model.Name);
                if (user != null)
                {
                        var result =
                                await _signInManager.PasswordSignInAsync(model.Name, model.Password, model.RememberMe, false);
                        if (result.Succeeded)
                        {
                                await _userManager.UpdateAsync(user);
                    Services.SetIdCurrentUser(user.Id);
                                return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Неверный логин или пароль");
                        }
                }
            else
            {
				ModelState.AddModelError("", "Пользователь с таким логином не зарегистрирован в системе, проверьте правильность ввдённых данных.");

			}

			return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Block(string id)
        {
            string IdUserNow = _userManager.GetUserAsync(HttpContext.User).Result.Id;

            BlockRecord blockRecord = new BlockRecord
            {
                IdWhoBlocked = IdUserNow,
                IdWhoDidBlock = id
            };
            await _context.AddAsync(blockRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Home");

        }
        [Authorize]
        public async Task<IActionResult> Unblock(string id)
        {
            string IdUserNow = _userManager.GetUserAsync(HttpContext.User).Result.Id;

            BlockRecord blockRecord = _context.BlockRecord.FirstOrDefault(r=>r.IdWhoDidBlock== id && r.IdWhoBlocked==IdUserNow);
             _context.Remove(blockRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Profile(string name)
        {
            var profileView = await _context.ProfileView
              .FirstOrDefaultAsync(pv => pv.UserName == name);

            profileView.FullUserCards= profileView.FullUserCards == null ? 0 : profileView.FullUserCards;
            profileView.FullUserLetter = profileView.FullUserLetter == null ? 0 : profileView.FullUserLetter;
            profileView.FullUserStamp = profileView.FullUserStamp == null ? 0 : profileView.FullUserStamp;

            return View(profileView);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ProfileView model, string userId, IFormFile picFromView)
        {
            User userFromDb = await _userManager.FindByIdAsync(userId);
            var name = userFromDb.UserName;
            if (model.Email != null)
                userFromDb.Email = model.Email;
            if (model.UserName != null)
                userFromDb.UserName = model.UserName;
            if (model.FirstName != null)
                userFromDb.FirstName = model.FirstName;
            if (model.LastName != null)
                userFromDb.LastName = model.LastName;
            if(picFromView != null)
            {
                userFromDb.Img = Services.ConvertPictureForDb(picFromView); 
            }
           
           _context.Users.Update(userFromDb);
            _context.SaveChanges();
            return RedirectToAction("Profile", "Account", new { name });
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string id)
        {
           
                User user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                //можно в бд просто поставить каскадное удаление
              var cards=  _context.UserCards.Where(u => u.IdUser == user.Id);
                _context.RemoveRange(cards);
                var stamp = _context.StampsUsers.Where(u => u.IdUser == user.Id);
                _context.RemoveRange(stamp);
                var message = _context.Messages.Where(u => u.SenderId == user.Id);
                _context.RemoveRange(message);
                var dialog = _context.Dialogs.Where(u => u.IdSender == user.Id || u.IdReceiver==user.Id);
                _context.RemoveRange(dialog);
                var blocks = _context.BlockRecord.Where(u => u.IdWhoDidBlock == user.Id || u.IdWhoBlocked == user.Id);
                _context.RemoveRange(blocks);
                _context.SaveChanges();
                IdentityResult result = await _userManager.DeleteAsync(user);
                }
            return RedirectToAction("Index", "Home");
        }
    }
}
