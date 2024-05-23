using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BelPost.ViewModels
{
    public class PostalLetterCardViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Название не заполнено")]
        public string Name { get; set; }
        public string? Letter { get; set; }
         [Required]
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public IFormFile ImgFromView { get; set; }
        [Required(ErrorMessage = "Номер по катологу не заполнен")]
        public int Number { get; set; }

    }
    public enum TypeCard
    {
        card,
        letter
    }
}
