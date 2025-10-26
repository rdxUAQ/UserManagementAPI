using System.ComponentModel.DataAnnotations;
using UserManagementAPI.Api.Validators;

namespace UserManagementAPI.Api.Models
{
    public class User
    {
        

        [Required]
        [IdGuid]
        public  string? Id{ get; set; }

        [Required]
        [EmailAddress]
        public  string? Email{ get; set; }

        [Required]
        public  string? Password{ get; set; }

        [Required]
        [RegularExpression("^[A-Za-z0-9 ]+$")]
        public  string? Fullname{ get; set; }

    }

}