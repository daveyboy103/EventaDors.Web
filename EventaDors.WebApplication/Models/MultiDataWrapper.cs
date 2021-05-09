using Microsoft.AspNetCore.Mvc.Rendering;

namespace EventaDors.WebApplication.Models
{
    public class MultiDataWrapper<T>
    {
        public SelectList List { get; set; }
        public T Single { get; set; }
    }

    public class EmailPayloadWrapper<T>
    {
        public T Payload { get; set; }
        public string Email { get; set; }
    }

    public class LinkWrapper
    {
        public string Text { get; set; }
        public string Link { get; set; }
    }
}