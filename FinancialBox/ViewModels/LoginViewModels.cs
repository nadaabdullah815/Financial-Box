using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace FinancialBox.ViewModels
{
public class LoginViewModel
{
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Required, DataType(DataType.Password)] public string Password { get; set; } = string.Empty;
   
}
}