using System;

namespace AlkemyChallenge.API.ViewModels.Auth.Login
{
    public class LoginResponseViewModel
    {
        public string Token { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
