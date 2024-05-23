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

namespace BelPost.Controllers
{
    public class PostalLetterCardsController : Controller
    {
        private readonly ApplicationContext _context;

        public PostalLetterCardsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: PostalLetterCards
        public async Task<IActionResult> Index(string type, DateTime? DateStart, DateTime? DateEnd)
        {
           DateEnd=DateEnd==null ? DateTime.MaxValue : DateEnd;
            DateStart = DateStart == null ? DateTime.MinValue : DateStart;
            if (DateStart > DateEnd)
            {
                var date = DateStart;
                DateStart = DateEnd;
                DateEnd=date;
            }
            ViewBag.Type = type;
                var stamps = await _context.PostalLetterCards.Where(p => p.Type == type && p.Date <= DateEnd && p.Date>=DateStart).ToListAsync();
                return View(stamps);
            
          
        }

        [Authorize]
        public async Task<IActionResult> IndexForUser(string? User, string type, DateTime? DateStart, DateTime? DateEnd)
        {
            DateEnd = DateEnd == null ? DateTime.MaxValue : DateEnd;
            DateStart = DateStart == null ? DateTime.MinValue : DateStart;
            if (DateStart > DateEnd)
            {
                var date = DateStart;
                DateStart = DateEnd;
                DateEnd = date;
            }
            ViewBag.nameUser = User;
            ViewBag.Type = type;
                var stamps = await _context.PostalLetterCards.Where(p => p.Type == type && p.Date <= DateEnd && p.Date >= DateStart).ToListAsync();
                return View(stamps);
        }
        // GET: PostalLetterCards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PostalLetterCards == null)
            {
                return NotFound();
            }

            var postalLetterCard = await _context.PostalLetterCards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postalLetterCard == null)
            {
                return NotFound();
            }

            return View(postalLetterCard);
        }


        public async Task<IActionResult> AddToUser(int idCard,string type)
        {
            var IdUser = User.Claims.Where(u => u.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.FirstOrDefault()?.Value;
            UserCard stampsUser2 =await _context.UserCards.FirstOrDefaultAsync(u => u.IdUser == IdUser && u.IdCardLetter == idCard);
            if (stampsUser2 == null)
            {
                stampsUser2 = new UserCard
                {
                    Count = 1,
                    IdCardLetter = idCard,
                    IdUser = IdUser
                };
                _context.UserCards.Add(stampsUser2);

            }
            else
            {
                stampsUser2.Count++;
                _context.UserCards.Update(stampsUser2);
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(IndexForUser), new { type });
        }

        public IActionResult DeleteToUser(int idCard, string type)
        {

            var IdUser = User.Claims.Where(u => u.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.FirstOrDefault()?.Value;
            UserCard stampsUser2 = _context.UserCards.FirstOrDefault(u => u.IdUser == IdUser && u.IdCardLetter == idCard);
            if (stampsUser2 != null || stampsUser2.Count >= 0)
            {
                stampsUser2.Count--;
                _context.UserCards.Update(stampsUser2);
                _context.SaveChanges();

            }

            return RedirectToAction(nameof(IndexForUser), new {type});
        }
        // GET: PostalLetterCards/Create
        public IActionResult Create(string type)
        {
            ViewBag.Type = type;
            return View();
        }

        // POST: PostalLetterCards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Letter,Date,Type,ImgFromView,Number")] PostalLetterCardViewModel postalLetterCardFromView)
        {
            if (ModelState.IsValid)
            {
                PostalLetterCard postalForDb = new PostalLetterCard
                {
                    Id = postalLetterCardFromView.Id,
                    Name = postalLetterCardFromView.Name,
                    Letter = postalLetterCardFromView.Letter,
                    Img = null,
                    Date = postalLetterCardFromView.Date,
                    Type= postalLetterCardFromView.Type,
                    Number= postalLetterCardFromView.Number
                };
                if (postalLetterCardFromView.ImgFromView != null)
                {
                    byte[] imageData = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(postalLetterCardFromView.ImgFromView.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)postalLetterCardFromView.ImgFromView.Length);
                    }
                    // установка массива байтов
                    postalForDb.Img = imageData;
                }

                _context.Add(postalForDb);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new {type= postalForDb.Type});
            }
            return View(postalLetterCardFromView);
        }

        // GET: PostalLetterCards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PostalLetterCards == null)
            {
                return NotFound();
            }

            var postalLetterCard = await _context.PostalLetterCards.FindAsync(id);
            ViewBag.Type = postalLetterCard.Type;
            if (postalLetterCard == null)
            {
                return NotFound();
            }
            return View(postalLetterCard);
        }

        // POST: PostalLetterCards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Number,CategoryId,Letter,Img, Date")] PostalLetterCard postalLetterCard, IFormFile picFromView)
        {
            var postalLetterCardFromDb = await _context.PostalLetterCards.FindAsync(id); // получаем объект из контекста

            if (postalLetterCardFromDb != null)
            {
                postalLetterCardFromDb.Letter = postalLetterCard.Letter;
                postalLetterCardFromDb.Name = postalLetterCard.Name;
                postalLetterCardFromDb.Date = postalLetterCard.Date;
                postalLetterCardFromDb.Number=postalLetterCard.Number;
                
                if (id != postalLetterCard.Id)
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

                        postalLetterCardFromDb.Img = imageData;
                    }
                    _context.Update(postalLetterCardFromDb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index), new {type = postalLetterCardFromDb.Type});
            }
            return View(postalLetterCard);
        }



        public async Task<IActionResult> Delete(int id)
        {
            if (_context.PostalLetterCards == null)
            {
                return Problem("Карточки ещё не выпущены");
            }
            var postalLetterCard = await _context.PostalLetterCards.FindAsync(id);
            if (postalLetterCard != null)
            {
                _context.PostalLetterCards.Remove(postalLetterCard);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostalLetterCardExists(int id)
        {
          return (_context.PostalLetterCards?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
