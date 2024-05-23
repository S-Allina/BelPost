using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BelPost.Models
{
    public class Message
    {
       
            public int Id { get; set; }
       
            public int DialogId { get; set; }
            public string SenderId { get; set; }
        [Required(ErrorMessage = "Нельзя отправить пустое сообщение")]
        public string Text { get; set; }
            public DateTime SentAt { get; set; }
            public Dialog Dialog { get; set; }
            public User Sender { get; set; }
        
    }
}
