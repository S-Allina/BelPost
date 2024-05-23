using BelPost.Models;
using Microsoft.AspNetCore.CookiePolicy;

namespace BelPost.Components
{
    public class CountFullViewComponent
    {
        private readonly ApplicationContext _context;


        public CountFullViewComponent(ApplicationContext context)
        {
            _context = context;
        }
        public string Invoke(string Name, bool isProfile)
        {
            //если компонент вызывается из профиля
            if (isProfile)
            {
                //получение сколько всего марок во всех листках
                var Vsego = (from ps in _context.PostageStamps
                             select ps.CountInCategory).Sum();
                //получение сколько марок у пользователя
                var UserPostageStamps = (from su in _context.StampsUsers
                                         join u in _context.Users on su.IdUser equals u.Id
                                         where u.UserName == Name
                                         select su.Count).Sum();

                 return $"{UserPostageStamps} / {Vsego}";
            }
            else
            {
                //получение сколько всего марок у пользователя
                var UserPostageStamps = (from su in _context.StampsUsers
                                         join u in _context.Users on su.IdUser equals u.Id
                                         where u.UserName == Name
                                         select su.Count).Sum();
                 return UserPostageStamps.ToString();
            }
        }
     
    }
}
