using Application.Account;

namespace Web.Models.Account
{
    public class RegisterViewModel
    {
        public RegisterInputDto Input { get; set; } = new RegisterInputDto();
        public string ReturnUrl { get; set; } = null!;
    }
}
