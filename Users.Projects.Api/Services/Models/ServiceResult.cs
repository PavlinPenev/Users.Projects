namespace Users.Projects.Api.Services.Models
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; set; }
        
        public T Data { get; set; }

        public int StatusCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}
