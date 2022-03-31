using Microsoft.AspNetCore.Identity;

namespace AlkemyChallenge.API.Models
{
    public class User : IdentityUser
    {
        public bool IsActive { get; set; }
    }    
}
