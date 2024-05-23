using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace BelPost.Models
{
    public class BlockRecord
    {
        [Key]
        public int IdBlockRecord { get; set; }
        public string IdWhoBlocked { get; set; }
        public string IdWhoDidBlock { get; set; }
    }
    }
