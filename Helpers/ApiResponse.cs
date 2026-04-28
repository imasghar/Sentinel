namespace Sentinel.Helpers
{
    public class ApiResponse<T>
    {
        public string Status { get; set; } = null!;
        public string Response { get; set; } = null!;
        public T? Result { get; set; }
        public List<T> ResponseList { get; set; } = new();
        public object? Data { get; set; }

    }
}
