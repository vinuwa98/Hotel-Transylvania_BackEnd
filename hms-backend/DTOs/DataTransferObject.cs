namespace HmsBackend.DTOs
{
    public class DataTransferObject<T>
    {
        public string? Message;
        public T? Data { get; set; }
    }
}
