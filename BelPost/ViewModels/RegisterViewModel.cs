
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BelPost.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email не заполнен")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Логин не заполнен")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Имя не заполнен")]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Фамилия не заполнен")]
        [Display(Name = "LastName")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Пароль не заполнен")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Повтор пароля не заполнен")]
        [Compare("Password", ErrorMessage = "Пароль не совподает")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string PasswordConfirm { get; set; }


        [Display(Name = "Img")]
        public IFormFile? ImgFromView { get; set; }
    }
}
