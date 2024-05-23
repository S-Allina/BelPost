using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BelPost.ViewModels
{
    public class PostageStampViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Название не заполнено")]
        public string Name { get; set; }

        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Буква не заполнена")]
        public string Letter { get; set; }
        [NotMapped]
        public IFormFile? ImgFromView { get; set; }
      public DateTime Date { get; set; }
        [Required(ErrorMessage = "Количество не заполнено")]
        public int CountInCategory { get; set; }



    }
}
