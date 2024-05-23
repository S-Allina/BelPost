using BelPost.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BelPost.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Название не заполнено")]
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? EqualFlag { get; set; }
        [NotMapped]
        public IFormFile ImgFromView { get; set; }
        public byte[]? Img { get; set; }

        public virtual ICollection<PostageStamp>? PostageStamps { get; set; }

    }
}
