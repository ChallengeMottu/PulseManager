namespace PulseManager.Models
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public List<Link> Links { get; set; } = new List<Link>();
    }

    public class Link
    {
        public string Rel { get; set; }
        public string Href { get; set; }
        public string Method { get; set; }
    }
}