
using BelPost.Models;
using BelPost.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace BelPost.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationContext _context;

        public HomeController(ApplicationContext context)
        {
            _context = context;
        }
        public ActionResult Index(string? isBlock)
        {
        
            List<SelectListItem> selectCategories = new List<SelectListItem> {
                new SelectListItem { Value = "-1", Text = "Все" }
                };

            List<SelectListItem> selectCategoriesFromDb = _context.Categories.ToList().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(), 
                Text = c.Name 
            }).ToList();
            selectCategories.AddRange(selectCategoriesFromDb);
            // Добавляем список категорий в ViewBag
            ViewBag.CategoryId = selectCategories;
            
            if (User.Identity.IsAuthenticated)
            {
                List<ProfileView> users = new List<ProfileView>();
                var idUser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Id;
                if (isBlock != null)
                {
                    ViewBag.isBlock = "yes";
                   users= _context.ProfileView.Where(u => u.IdWhoBlocked.Contains(idUser))
                   .ToList();
                }
                else
                {
                     users = _context.ProfileView.Where(u => u.UserName != User.Identity.Name)
                        .OrderByDescending(u => u.FullUserStamp)
                        .ToList();
                }
                ViewBag.IdUser = idUser;
                return View(users);
            }
            else
            {
                var users = _context.ProfileView
                   .OrderByDescending(u => u.FullUserStamp)
                   .ToList();

                return View(users);
            }
        }

        public ActionResult SearchView()
        {
            return PartialView("_SearchView");
        }

        public ActionResult Filtr(string userName = "", string userSurname = "", string CategoryId = "")
        {
            List<SelectListItem> selectCategories = new List<SelectListItem>{
                new SelectListItem { Value = "-1", Text = "Все" } };
            List<SelectListItem> selectCategoriesFromDb = _context.Categories.ToList().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(), // Здесь должно быть строковое значение идентификатора категории
                Text = c.Name // Здесь должно быть название категории
            }).ToList();
            selectCategories.AddRange(selectCategoriesFromDb);
            ViewBag.CategoryId = selectCategories;

            CategoryId = CategoryId == "все" || CategoryId == "-1" ? "" : CategoryId;

            List<ProfileView> users = new List<ProfileView>();

            if (CategoryId != "")
            {
                users = _context.ProfileView.Where(pv => EF.Functions.Like(pv.FirstName, $"%{userName}%") &&
               EF.Functions.Like(pv.LastName, $"%{userSurname}%") && EF.Functions.Like(pv.Cat, $"%{CategoryId}%")).ToList();
            }
            else
            {
                users = _context.ProfileView.Where(pv => EF.Functions.Like(pv.FirstName, $"%{userName}%") &&
                           EF.Functions.Like(pv.LastName, $"%{userSurname}%")).ToList();
            }
            return View("Index", users);
        }


        public ActionResult IndexTop(string? type)
        {
            List<ProfileView> users = new List<ProfileView>();
            if (type == "Letter")
            {
                users = _context.ProfileView
                   .OrderByDescending(u => u.FullUserLetter)
                   .ToList();
                
                ViewBag.Full = _context.PostalLetterCards.Where(c => c.Type == "Letter").Count();
                ViewBag.type = "Letter";
            }
            else if (type == "Card")
            {
                users = _context.ProfileView
                 .OrderByDescending(u => u.FullUserCards)
                 .ToList();
                ViewBag.Full = _context.PostalLetterCards.Where(c => c.Type == "Card").Count();
                ViewBag.type = "Card";
            }
            else
            {
                users = _context.ProfileView
                       .OrderByDescending(u => u.FullUserStamp)
                       .ToList();
                ViewBag.Full = _context.PostageStamps.Select(p=>p.CountInCategory).Sum();
                ViewBag.type = "Stamp";
            }
            if (User.Identity.IsAuthenticated)
            {
                var idUser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Id;
                ViewBag.IdUser = idUser;
            }
            return View(users);
        }

       




    }
}
