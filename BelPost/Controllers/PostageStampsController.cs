using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BelPost.Models;
using BelPost.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using BelPost.Service;

namespace BelPost.Controllers
{
    public class PostageStampsController : Controller
    {
        private readonly ApplicationContext _context;
        public PostageStampsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: PostageStamps
        public async Task<IActionResult> Index(DateTime? DateStart, DateTime? DateEnd)
        {
            DateEnd = DateEnd == null ? DateTime.MaxValue : DateEnd;
            DateStart = DateStart == null ? DateTime.MinValue : DateStart;
            if (DateStart > DateEnd)
            {
                var date = DateStart;
                DateStart = DateEnd;
                DateEnd = date;
            }
            var stamps = await _context.PostageStamps.Where(p => p.Date<=DateEnd && p.Date>=DateStart).ToListAsync();
                return View(stamps);
           
             
        }

        [Authorize]
        public async Task<IActionResult> IndexForUser(string? User, int? idCategory, DateTime? DateStart, DateTime? DateEnd)
        {
            List<PostageStamp> stamps = null;
            ViewBag.nameUser = User;
            Category category =await _context.Categories.FirstOrDefaultAsync(c=>c.Id==idCategory);

            if(category==null) return NotFound();

            DateEnd = DateEnd == null ? DateTime.MaxValue : DateEnd;
            DateStart = DateStart == null ? DateTime.MinValue : DateStart;
            if (DateStart > DateEnd)
            {
                var date = DateStart;
                DateStart = DateEnd;
                DateEnd = date;
            }
            if (idCategory != null)
            {
                 stamps = await _context.PostageStamps.Where(p => p.CategoryId == idCategory && p.Date <= DateEnd && p.Date >= DateStart).ToListAsync();
            }
            else
            {
                stamps = await _context.PostageStamps.ToListAsync();
            }

            category.PostageStamps= stamps;
            return View(category);
        }
      
        // GET: PostageStamps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PostageStamps == null)
            {
                return NotFound();
            }

            var postageStamp = await _context.PostageStamps
                .FirstOrDefaultAsync(m => m.Id == id);

            ViewBag.Category = _context.Categories.FirstOrDefault(c => c.Id == postageStamp.CategoryId).Name;
            if (postageStamp == null)
            {
                return NotFound();
            }

            return View(postageStamp);
        }

        [Authorize]
        public async Task<IActionResult> AddToUser(int idStamp)
        {
            if (_context != null)
            {
                var IdUser = User.Claims.Where(u => u.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.FirstOrDefault()?.Value;

                StampsUser stampsUser2 = await _context.StampsUsers.FirstOrDefaultAsync(u => u.IdUser == IdUser && u.IdPostageStamp == idStamp);

                int idCategory = _context.PostageStamps.FirstOrDefault(c => c.Id == idStamp).CategoryId;
                if (stampsUser2 == null)
                {
                    stampsUser2 = new StampsUser
                    {
                        Count = 1,
                        IdPostageStamp = idStamp,
                        IdUser = IdUser
                    };

                    await _context.StampsUsers.AddAsync(stampsUser2);
                }
                else
                {
                    stampsUser2.Count++;
                    _context.StampsUsers.Update(stampsUser2);
                }
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(IndexForUser), new { idCategory });
            }
            return NotFound();
        
        }

        [Authorize]
        public async Task<IActionResult> DeleteToUser(int idStamp)
        {

            var IdUser = User.Claims.Where(u => u.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.FirstOrDefault()?.Value;
            StampsUser stampsUser2 = _context.StampsUsers.FirstOrDefault(u => u.IdUser == IdUser && u.IdPostageStamp == idStamp);
            
            int idCategory = _context.PostageStamps.FirstOrDefault(c => c.Id == idStamp).CategoryId;

            if ( stampsUser2!=null|| stampsUser2.Count >= 0)
            {
                stampsUser2.Count--;
                _context.StampsUsers.Update(stampsUser2);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(IndexForUser), new { idCategory });
        }

        // GET: PostageStamps/Create
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create()
        {
            var categories =await _context.Categories.ToListAsync();

            // Создаем список объектов SelectListItem для заполнения выпадающего списка
            var selectListItems = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(), // Здесь должно быть строковое значение идентификатора категории
                Text = c.Name // Здесь должно быть название категории
            }).ToList();

            // Добавляем список категорий в ViewBag
            ViewBag.CategoryId = selectListItems;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CountInCategory,CategoryId,Letter,ImgFromView,Date")] PostageStampViewModel postageStampFromView)
        {
            if (ModelState.IsValid)
            {
                PostageStamp postageFromDb = new PostageStamp
                {
                    Id= postageStampFromView.Id,
                    Name= postageStampFromView.Name,
                    CategoryId= postageStampFromView.CategoryId,
                    Letter= postageStampFromView.Letter,
                    Img=null,
                    CountInCategory= postageStampFromView.CountInCategory,
                    Date= postageStampFromView.Date
                };

                if (postageStampFromView.ImgFromView != null)
                {                    
                    postageFromDb.Img = Services.ConvertPictureForDb(postageStampFromView.ImgFromView);
                }

               await _context.AddAsync(postageFromDb);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Categories", new { id = postageFromDb.CategoryId });
            }
            return View(postageStampFromView);
        }

        // GET: PostageStamps/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PostageStamps == null)
            {
                return NotFound();
            }

            var categories =await _context.Categories.ToListAsync();

            // Создаем список объектов SelectListItem для заполнения выпадающего списка
            var selectListItems = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(), // Здесь должно быть строковое значение идентификатора категории
                Text = c.Name // Здесь должно быть название категории
            }).ToList();

            // Добавляем список категорий в ViewBag
            ViewBag.CategoryId = selectListItems;

            var postageStamp = await _context.PostageStamps.FindAsync(id);
            if (postageStamp == null)
            {
                return NotFound();
            }
            return View(postageStamp);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CountInCategory,CategoryId,Letter,Img, Date")] PostageStamp postageStamp, IFormFile picFromView)
        {
            var postageStampFromDb = await _context.PostageStamps.FindAsync(id); // получаем объект из контекста

            if (postageStampFromDb != null)
            {
                postageStampFromDb.Letter = postageStamp.Letter;
                postageStampFromDb.Name = postageStamp.Name;
                postageStampFromDb.CategoryId = postageStamp.CategoryId;
                postageStampFromDb.Date=postageStamp.Date;
                postageStampFromDb.CountInCategory=postageStamp.CountInCategory;
                if (id != postageStamp.Id)
                {
                    return NotFound();
                }

                try
                {
                    if (picFromView != null)
                    {
                        byte[] imageData = null;

                        using (var binaryReader = new BinaryReader(picFromView.OpenReadStream()))
                        {
                            imageData = binaryReader.ReadBytes((int)picFromView.Length);
                        }

                        postageStampFromDb.Img = imageData;
                    }
                    _context.Update(postageStampFromDb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                        return NotFound();
                }
                return RedirectToAction("Index","Categories");
            }
            return View(postageStamp);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.PostageStamps == null)
            {
                return Problem("Марок нет");
            }
            var postageStamp = await _context.PostageStamps.FindAsync(id);
            if (postageStamp != null)
            {
                _context.PostageStamps.Remove(postageStamp);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
