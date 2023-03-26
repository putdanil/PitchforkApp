using System.ComponentModel.DataAnnotations;

namespace NLayerApp.DAL.Entities;
    public class LoginModel
    {
        [StringLength(100)]
        [Required(ErrorMessage = "Please Enter Login")]
        [Display(Name = "Login")]
        public string Login { get; set; } = null!;

        [StringLength(100)]
        [Required(ErrorMessage = "Please Enter Password")]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;
    }
