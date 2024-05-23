using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BelPost.Models
{
    public class PostalLetterCard
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Название не заполнено")]
        public string Name { get; set; }
        public string? Letter { get; set; }
        [Required(ErrorMessage = "Дата выпуска не заполнена")]
        public DateTime Date { get; set; }

        public string Type { get; set; }
        public byte[]? Img { get; set; }
        [Required(ErrorMessage = "Номер по катологу не заполнен")]
        public int Number { get; set; }

    }

}
