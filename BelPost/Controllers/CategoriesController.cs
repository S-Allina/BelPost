using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BelPost.Models;
using BelPost.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using BelPost.Service;

namespace BelPost.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationContext _context;

        public CategoriesController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
              return _context.Categories != null ? 
                          View(await _context.Categories.ToListAsync()) :
                          Problem("Информационные листки ещё не выпущены");
        }

        [Authorize]
        public async Task<IActionResult> IndexForUser(string? nameUser)
        {
            if (nameUser != null)
            {
                nameUser = Services.GetIdCurrentUser();

                ViewBag.nameUser = nameUser;
            }
            else
            {
                nameUser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Id;
            }
            var IdUser2 = Services.GetIdCurrentUser();

            var cat =await _context.Categories.ToListAsync();
            var catView =new List<CategoryViewModel>();
            foreach (var t in cat)
            {
                
                var categoryViewItemForThisUser = _context.CategoryView.FirstOrDefault(c => c.IdUser == nameUser && c.CategoryId == t.Id && c.EqualFlag == 1);
                catView.Add(new CategoryViewModel
                {
                    Name= t.Name,
                    Id=t.Id,
                    Description=t.Description,
                    PostageStamps=t.PostageStamps,
                    Img = t.Img,
                    EqualFlag= categoryViewItemForThisUser == null   ? 0 : 1,
                });
            }
            return  View(catView);
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            category.PostageStamps= await _context.PostageStamps.Where(s=>s.CategoryId== id).ToListAsync();

            return View(category);
        }

        // GET: Categories/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,ImgFromView")] CategoryViewModel categoryFromView)
        {
            if (ModelState.IsValid)
            {
                Category categoryForDb = new Category
                {
                    Id = categoryFromView.Id,
                    Name = categoryFromView.Name,
                    Description = categoryFromView.Description,
                    Img=null
                };

                if (categoryFromView.ImgFromView != null)
                {
                    categoryForDb.Img = Services.ConvertPictureForDb(categoryFromView.ImgFromView);
                }

               await _context.Categories.AddAsync(categoryForDb);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoryFromView);
        }

        // GET: Categories/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ImageStorageName,Img")] Category category, IFormFile picFromView)
        {
            var categoryFromDB = await _context.Categories.FindAsync(id); // получаем объект из контекста

            if (categoryFromDB != null)
            {
                categoryFromDB.Name = category.Name;
                categoryFromDB.Description = category.Description;
                if (id != category.Id)
                {
                    return NotFound();
                }

                try
                {
                    if (picFromView != null)
                    {
                        categoryFromDB.Img = Services.ConvertPictureForDb(picFromView);
                    }
                    _context.Update(categoryFromDB);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                   
                        return NotFound();
                    
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Листков нет");
            }

            // Получаем все марки, связанные с категорией
            var stamps = await _context.PostageStamps.Where(p => p.CategoryId == id).ToListAsync();

            // Удаляем записи из StampsUsers, связанные с этими марками
            foreach (var stamp in stamps)
            {
                var stampsUser = _context.StampsUsers.Where(p => p.IdPostageStamp == stamp.Id);
                _context.StampsUsers.RemoveRange(stampsUser);
            }
            await _context.SaveChangesAsync();

            // Удаляем сами марки
            _context.PostageStamps.RemoveRange(stamps);
            await _context.SaveChangesAsync();

            // Удаляем категорию
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
