using BelPost.Models;
using Microsoft.AspNetCore.CookiePolicy;

namespace BelPost.Components
{
    public class CountCategoryViewComponent
    {
        private readonly ApplicationContext _context;


        public CountCategoryViewComponent(ApplicationContext context)
        {
            _context = context;
        }
        public string Invoke(int idCategory, string? Name, string? type)
        {
            var isUser = _context.Users.FirstOrDefault(u => u.UserName == Name);
             Name = isUser == null ? Name : isUser.Id;
            //если это марки
            if (type == null)
            {
                //получение сколько всего таких в категории
                var Vsego = (from ps in _context.PostageStamps where ps.CategoryId == idCategory
                             select ps.CountInCategory).Sum();

                
                // Получение количества марок у пользователя с idUser = 5 для марки с idPostageStamp = 10
                var UserPostageStamps = (from su in _context.StampsUsers
                                         join ps in _context.PostageStamps on su.IdPostageStamp equals ps.Id
                                         where su.IdUser == Name && ps.CategoryId == idCategory
                                         select su.Count).Sum();

                return $"{UserPostageStamps} из {Vsego}";
            
            }
            //если это конверты или карточки
            else 
            {
                //получение количества у пользователя
                var UserPostageStamps = (from su in _context.UserCards
                                         join ps in _context.PostalLetterCards on su.IdCardLetter equals ps.Id
                                         where su.IdUser == Name
                                         select su.Count)
                                         .SingleOrDefault();
                
                return $"{UserPostageStamps}";
            }
        }
     
    }
}
