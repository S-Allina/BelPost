using BelPost.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BelPost.ViewModels
{
    public class CategoryForUserViewModel
    {
        [Key]
        public int CategoryId { get; set; }
        public string IdUser { get; set; }
        public int? EqualFlag { get; set; }


    }
}
