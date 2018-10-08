using Microsoft.AspNetCore.Identity;

namespace PracticeApp.Models
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
    }
}
