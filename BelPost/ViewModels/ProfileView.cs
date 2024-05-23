using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BelPost.ViewModels
{
    public partial class ProfileView
    {
        public string Id { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string? UserName { get; set; }

        public string? Email { get; set; }

        public byte[]? Img { get; set; }
        public int? FullUserCards { get; set; }

        public int? FullUserStamp { get; set; }
        public int? FullUserLetter { get; set; }
        public int? UserCollection { get; set; }
        public string? IdWhoBlocked { get; set; }
        public string? IdWhoDidBlock { get; set; }  
        public string? Cat { get; set; }
    }


}
