using Application.Account;

namespace Web.Models.Account
{
    public class LoginViewModel
    {
        public LoginInputDto Input { get; set; } = new LoginInputDto();
        public string ReturnUrl { get; set; } = null!;
    }
}
