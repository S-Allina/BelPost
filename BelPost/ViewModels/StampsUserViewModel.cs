using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BelPost.ViewModels
{
    public class StampsUserViewModel
    {
        public int Id { get; set; }
        public int IdPostageStamp { get; set; }
        public string IdUser { get; set; }
        [Required(ErrorMessage = "Количество не заполнено")]
        public int Count { get; set; }
    }
}
