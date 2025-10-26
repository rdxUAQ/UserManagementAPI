namespace UserManagementAPI.Base
{
    public class BaseResponse<T>
    {
        public bool? Ok { get => string.IsNullOrEmpty(Message); }
        public T? Data { get; set; }
        public string? Message { get => Error?.Description; }
        public BaseError? Error { get; set; }
    }
    
}