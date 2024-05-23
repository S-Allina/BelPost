using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BelPost.Models
{
    public class PostageStamp
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Название не заполнено")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Информационный листок не выбран")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Буква не заполнена")]
        public string Letter { get; set; }
        public byte[]? Img { get; set; }
        [Required(ErrorMessage = "Дата выпуска не заполнена")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Количество не заполнено")]
        public int CountInCategory { get; set; }



    }
}
