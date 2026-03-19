using System.ComponentModel.DataAnnotations;

namespace CS_maturita_test.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Uživatelské jméno")]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Heslo")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Zapamatovat si mě")]
        public bool RememberMe { get; set; }
    }
}
