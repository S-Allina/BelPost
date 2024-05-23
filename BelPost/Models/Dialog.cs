using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BelPost.Models
{
    public class Dialog
    {
        public int Id { get; set; }
        public string IdSender { get; set; }
        public string IdReceiver { get; set; }
        public User Sender { get; set; }
        public User Receiver { get; set; }
    }
    }
