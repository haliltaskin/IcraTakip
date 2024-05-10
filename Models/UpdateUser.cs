using System.ComponentModel.DataAnnotations;

namespace IcraTakipProgrami.Models
{
    public class UpdateUser
    {
        public string? Id { get; set; }
        public string? UserName { get; set; } 

        [EmailAddress]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Parola eşleşmiyor")]
        public string? ConfirmPassword { get; set; }

        public IList<string?> SelectedRoles { get; set; }




    }
}
