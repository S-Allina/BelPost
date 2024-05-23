using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BelPost.Models
{
    public class StampsUser
    {
        public int Id { get; set; }
        public int IdPostageStamp { get; set; }
        public string IdUser { get; set; }
        public int Count { get; set; }


    }
}
