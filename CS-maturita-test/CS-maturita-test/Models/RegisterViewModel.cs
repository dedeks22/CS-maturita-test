using System.ComponentModel.DataAnnotations;

namespace CS_maturita_test.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Uživatelské jméno")]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Heslo")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Potvrzení hesla")]
        [Compare(nameof(Password), ErrorMessage = "Hesla se neshodují.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
