namespace UserManagementAPI.Base
{
    public class BaseError
    {
        public string? Code { get; set; }           // e.g., "InvalidEmail", "MissingField"
        public string? Description { get; set; }        // Human-readable error message
        public string? Target { get; set; }  
    }
}