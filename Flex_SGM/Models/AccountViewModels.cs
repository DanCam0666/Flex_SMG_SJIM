﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Flex_SGM.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Código")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "¿Recordar este explorador?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Usuario")]
       // [EmailAddress]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "¿Recordar cuenta?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
       // [EmailAddress]
        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El numero de caracteres de {0} debe ser al menos {2}.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }
        [Required]
        [MaxLength(20)]
        public string Nomina { get; set; }
        [Required]
        [Display(Name = "Area Asignada")]
        [MaxLength(50)]
        public string Area { get; set; }
        [Required]
        [Display(Name = "Puesto Asignado")]
        [MaxLength(50)]
        public string Puesto { get; set; }
        [Required]
        [Display(Name = "Nombre Completo")]
        public string UserFullName { get; set; }

        [Display(Name = "Correo Electronico")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Departamento Asignado")]
        [MaxLength(50)]
        public string Departamento { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
      //  [EmailAddress]
        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El numero de caracteres de {0} debe ser al menos {2}.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        public string Nomina { get; set; }

        public string Puesto { get; set; }
    }
}
