namespace ToDoListAPI.Helpers
{
    public class ApiResponse<T>
    {
        //public ApiResponse(T data)
        //{
        //    Data = data;
        //}

        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
}
