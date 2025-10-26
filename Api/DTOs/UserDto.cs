using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using UserManagementAPI.Api.Validators;

namespace UserManagementAPI.Api.Models.Dtos
{

    public class UserDto
    {
        [IdGuid]
        public string? Id { get; set; }
        
        [EmailAddress]
        public string? Email { get; set; }

        [MinLength(6)]
        public string? Password { get; set; }

        [RegularExpression("^[A-Za-z0-9 ]+$")]
        public string? Fullname { get; set; }

        // Capture any additional JSON properties sent by the client.
        // Controller will reject requests that include unexpected properties.
        [JsonExtensionData]
        public Dictionary<string, JsonElement>? ExtensionData { get; set; }
    }
    
        

}