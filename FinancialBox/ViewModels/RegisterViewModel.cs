using System;
using System.ComponentModel.DataAnnotations;

namespace FinancialBox.ViewModels
{
public class RegisterViewModel
{
    [Required] public string FullName { get; set; } = string.Empty;
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Required, DataType(DataType.Password)] public string Password { get; set; } = string.Empty;
}


}