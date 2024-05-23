using BelPost.Models;
using Microsoft.AspNetCore.CookiePolicy;

namespace BelPost.Components
{
    public class CountViewComponent
    {
        private readonly ApplicationContext _context;


        public CountViewComponent(ApplicationContext context)
        {
            _context = context;
        }
        public string Invoke(int idStamp, string Name, string? type)
        {
            var isUser = _context.Users.FirstOrDefault(u => u.UserName == Name);
            Name = isUser == null ? Name : isUser.Id;
            //если это марка
            if (type == null)
            {
                //получение сколько таких марок должно быть в листке
                var Vsego = (from ps in _context.PostageStamps where ps.Id == idStamp
                             select ps.CountInCategory).SingleOrDefault();

                // Получение количества таких марок у пользователя 
                var UserPostageStamps = (from su in _context.StampsUsers
                                         join ps in _context.PostageStamps on su.IdPostageStamp equals ps.Id
                                         join c in _context.Categories on ps.CategoryId equals c.Id
                                         join u in _context.Users on su.IdUser equals u.Id
                                         where u.Id == Name && su.IdPostageStamp== idStamp
                                         select su.Count).SingleOrDefault();


                return $"{UserPostageStamps} из {Vsego}";
            }
            //если это конверты или карточки
            else 
            {
                //получаем сколько таких у пользователя
                var UserPostageStamps = (from su in _context.UserCards
                                         join ps in _context.PostalLetterCards on su.IdCardLetter equals ps.Id
                                         join u in _context.Users on su.IdUser equals u.Id
                                         where u.Id == Name && su.IdCardLetter==idStamp
                                         //&& ps.Id == idCategory 
                                         select su.Count)
                                         .SingleOrDefault();
                return $"{UserPostageStamps}";
                
            }
        }
     
    }
}
