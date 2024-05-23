using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BelPost.Models
{
    public class User :IdentityUser
    {
        [Required(ErrorMessage = "Фамилия не заполнена")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Имя не заполнено")]
        public string FirstName { get; set; }

       
        public byte[]? Img { get; set; }
    }
}
