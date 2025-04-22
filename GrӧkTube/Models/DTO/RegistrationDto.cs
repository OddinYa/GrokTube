using System.ComponentModel.DataAnnotations;

namespace GrӧkTube.Models.DTO
{
    public class RegistrationDto
    {
        [Required(ErrorMessage = "Имя обязательно")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Имя должно быть от 2 до 50 символов")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Логин обязателен")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Логин должен быть от 3 до 20 символов")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Только буквы, цифры и подчеркивание")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Пароль обязателен")]
        [StringLength(100, ErrorMessage = "Пароль должен быть от {2} до {1} символов", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Подтверждение пароля обязательно")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Выберите расу")]
        [StringLength(20, ErrorMessage = "Название расы слишком длинное")]
        public string Race { get; set; }


        [Required(ErrorMessage = "Пожалуйста, нарисуйте цифру")]
        public string CaptchaDrawnDigit { get; set; }

        public string CaptchaExpectedDigit { get; set; }
    }
}
