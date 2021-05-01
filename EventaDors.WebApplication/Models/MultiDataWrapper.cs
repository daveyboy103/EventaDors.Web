using Microsoft.AspNetCore.Mvc.Rendering;

namespace EventaDors.WebApplication.Models
{
    public class MultiDataWrapper<T>
    {
        public SelectList List { get; set; }
        public T Single { get; set; }
    }
}